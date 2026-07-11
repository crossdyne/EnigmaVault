import { Component, effect, inject, model, signal, viewChild } from "@angular/core";
import { TagService } from "../../services/tag.service";
import { TagResponse } from "../../models/tag.response";
import { ErrorList, Result } from "@crossdyne/toolkit";
import { TagsListComponent } from "../../../../shared/ui/tags/tags-list.component";
import { ItemInputActionsComponent } from "../../../../shared/ui/item-input-actions/item-input-actions.component";
import { CreateTagRequest } from "../../models/create-tag.request";
import { UpdateTagRequest } from "../../models/update-tag.request";
import { IconCategoryResponse } from "../../models/icon-category.response";
import { IconCategoryService } from "../../services/icon-category.service";
import { ComboboxComponent } from "../../../../shared/ui/combobox/combobox.component";
import { IconsComponent } from "../../../../shared/ui/icons/icons.component";
import { AssetUrlResponse } from "../../models/asset-urls.response";
import { AssetService } from "../../services/asset.service";
import { EncryptedVaultResponse } from "../../models/encrypted-vault.response";
import { VaultService } from "../../services/vault.service";
import { VaultItemDisplay } from "../../models/domain/vault-item-display";
import { IconUrl } from "../../models/icon-url";
import { CryptoService } from "@crossdyne/security";
import { OverviewPayload } from "../../models/domain/overview-payload";
import { CryptoStateService } from "../../../../core/services/crypto-state.service";
import { Router } from "@angular/router";
import { OverlayModule } from "@angular/cdk/overlay";
import { CdkContextMenuTrigger, CdkMenu, CdkMenuItem, CdkMenuTrigger } from "@angular/cdk/menu";
import { Dialog } from "@angular/cdk/dialog";
import { CreateVaultComponent } from "../../components/form/vault-item-form.component";
import { CreateVaultItemRequest } from "../../models/dto/create-vault.request";
import { CreateVaultItemModalData } from "../../models/modal/create-vault-item.modal-data";
import { VaultTypeEnum } from "../../models/domain/vault-type.enum";
import { CreateVaultItemResult } from "../../models/modal/create-vault-item.result";
import { VaultCryptoService } from "../../services/crypto-vault.service";

@Component({
    selector: 'passwords-page',
    templateUrl: './passwords.page.html',
    styleUrls: ['./passwords.page.scss'],
    standalone: true,
    imports: [
        TagsListComponent, 
        ItemInputActionsComponent, 
        ComboboxComponent, 
        IconsComponent,
        OverlayModule,
        CdkMenu,
        CdkMenuItem,
        CdkMenuTrigger,
        CdkContextMenuTrigger
    ]
})
export class PasswordsPage {
    private router = inject(Router);
    private dialog = inject(Dialog);
    private tagService = inject(TagService);
    private iconCategoryService = inject(IconCategoryService);
    private assetService = inject(AssetService);
    private vaultService = inject(VaultService);
    private vaultCryptoService = inject(VaultCryptoService);
    private cryptoStateService = inject(CryptoStateService);

    // private cryptoService = new CryptoService();
   
    constructor() {
        this.initAsync();

        effect(() => {
            const cat = this.selectedCategory();
            
            if (!cat)
                return;
            
            console.log('Выбранная категория: ', cat.name);
        });
        
        effect(() =>{
            const icon = this.selectedIcon();
            
            if (!icon)
                return;
            
            console.log('Выбрана иконка с именем: ', icon.assetName);
        });
    }
    
    private async initAsync() {
        await this.getTagsAsync();
        await this.getIconCategoriesAsync();
        await this.getIcons();
        await this.getVaults();
    }

    trigger = viewChild.required<CdkMenuTrigger>('trigger')

    activeTab = signal<'tags' | 'icons'>('tags');
    
    setActiveTab(tab: 'tags' | 'icons') {
        this.activeTab.set(tab);
    }

    //#region Vaults

    vaults = signal<VaultItemDisplay[]>([]);
 
    openAddVaultItem() {
        const dialogRef = this.dialog.open<CreateVaultItemResult, CreateVaultItemModalData, CreateVaultComponent>(
            CreateVaultComponent, { 
                width: '500px',
                disableClose: false,
                hasBackdrop: true,
                backdropClass: 'custom-backdrop'
            }
        );
        
        dialogRef.closed.subscribe(async (result) => {
            if (!result) return;
            
            console.log('Тип:', result.type);
            console.log('Общие поля:', result.common);
            console.log('Детали:', result.details);

            const encryptedOVerView = await this.vaultCryptoService.encryptOverview({ 
                ServiceName: result.common.name,
                Url: result.common.url,
                Note: result.common.description
            });

            const encryptedDetails = await this.vaultCryptoService.encryptDetails(result.details);

            let iconId: string;

            switch (result.type) {
                case VaultTypeEnum.Password:
                    iconId = '1b0e32c3-ba08-423d-bb76-fb6e982c122e';
                    break;
                case VaultTypeEnum.ApiKey:
                    iconId = '41cbdb12-30f0-48d8-8932-704b10519fda';
                    break;
                case VaultTypeEnum.CreditCard:
                    iconId = 'd2af044d-d576-40e7-80c6-cb2bf9176f4d';
                    break;
                case VaultTypeEnum.Server:
                    iconId = '9c75d52a-347f-4a90-a96c-663509e34e26';
                    break;
            
                default:
                    iconId = '5e3e7328-12b7-4740-ad90-90889e15b58e';
                    break;
            }

            const request: CreateVaultItemRequest = {
                passwordType: result.type,
                iconId: iconId,
                encryptedOverview: encryptedOVerView,
                encryptedDetails: encryptedDetails
            };

            const resultCreated: Result<string> = await this.vaultService.createAsync(request);

            resultCreated.match(
                id => {
                    console.log('Запись успешно создана!');
                },
                errors => console.error(this.mapErrors(errors))
            );
        });
    }

    // CRUD
    async getVaults() {
        const dek = this.cryptoStateService.dek;
        if (!dek) {
            this.router.navigate(['/passwords/access']);
            return;
        }

        const result: Result<EncryptedVaultResponse[]> = await this.vaultService.getAllAsync();

        result.match(
            async vaults => {
                const vaultItems: VaultItemDisplay[] = [];

                for (const vault of vaults) {
                    
                    const fullTags: TagResponse[] = this.tags().filter(t => vault.tagsIds.includes(t.id));
                    const iconUrl = this.icons().find(i => i.assetId === vault.iconId);

                    let icon: IconUrl | undefined = undefined;
                    if (iconUrl)
                        icon = { id: iconUrl?.assetId, url: iconUrl?.url }

                    const overview: OverviewPayload | null = await this.vaultCryptoService.decryptOverview(vault.encryptedOverview);

                    const vaultItem: VaultItemDisplay = {
                        id: vault.id,
                        type: vault.type as VaultTypeEnum,
                        serviceName: overview?.ServiceName!,
                        url: overview?.Url!,
                        dateAdded: vault.dateAdded,
                        dateUpdate: vault.dateUpdate,
                        deletedAt: vault.deletedAt,
                        isFavorite: vault.isFavorite,
                        isArchive: vault.isArchive,
                        isInTrash: vault.isInTrash,
                        encryptedOverview: vault.encryptedOverview,
                        encryptedDetails: vault.encryptedDetails,
                        tags: fullTags,
                        icon: icon,
                        note: overview?.Note!
                    }

                    vaultItems.push(vaultItem);
                    console.log(overview?.ServiceName);
                }
                this.vaults.set(vaultItems);
            },
            async errors => console.error('Ошибка получение паролей: ', this.mapErrors(errors))
        );
    }

    // Actions
    onMoveToArchiveVault(vault: VaultItemDisplay) {
        console.log('Архивируем:', vault.id);
    }

    onCopyVault(vault: VaultItemDisplay) {
        console.log('Копируем пароль для:', vault.serviceName);
    }

    async onMoveToTrashVault(vault: VaultItemDisplay) {
        console.log('В корзину:', vault.id);
    }

    async onChangeFavorite(vault: VaultItemDisplay) {
        if (vault.isFavorite){
            vault.isFavorite = false;
            console.log('Удалено из избранного: ', vault.serviceName);
        } else {
            vault.isFavorite = true;
            console.log('Добавлено в избранное: ', vault.serviceName);
        }
    }

    //#endregion

    //#region Tags

    tags = signal<TagResponse[]>([]);
    selectedTag = signal<TagResponse | null>(null);
    editingTag = signal<TagResponse | null>(null);

    getTagName = (tag: TagResponse) => tag.name;
    setTagName = (tag: TagResponse, name: string) => ({ ...tag, name});

    //Events & Selectors

    onSelectedTag(tag: TagResponse) {
        this.selectedTag.set(tag);
    }
    
    onEditTag(tag: TagResponse) {
        this.editingTag.set(tag);
    }

    onCancelEditTag() {
        this.editingTag.set(null);
    }

    // CRUD
    async getTagsAsync() {
        const result: Result<TagResponse[]> = await this.tagService.getAllAsync();

        result.match(
            tags => this.tags.set(tags),
            errors => console.log(this.mapErrors(errors))
        );
    } 

    async onCreateTag(name: string) {
        let request: CreateTagRequest = {
            id: '',
            name: name,
            color: '#F0F0F0'
        } 

        const result: Result<string> = await this.tagService.createAsync(request);

        result.match(
            id => {
                request.id = id;
                const update = [request, ...this.tags()];
                this.tags.set(update);
            },
            errors => console.log(this.mapErrors(errors))
        );
    }

    async onUpdateTag(tag: TagResponse) {
        const request: UpdateTagRequest = {
            id: this.editingTag()?.id as string,
            name: tag.name,
            color: tag.color
        }

        const result: Result = await this.tagService.updateAsync(request);

        result.match(
            () => {
                const id = this.editingTag()?.id;

                if (!id)
                    return;

                const updatedTag: TagResponse = {
                    id: this.editingTag()?.id as string,
                    name: tag.name,
                    color: tag.color
                } 

                this.tags.update(tags => tags.map(t => t.id === updatedTag.id ? updatedTag : t));  
            },
            errors => console.error('Ошибка обновления: ', this.mapErrors(errors))
        );

        this.editingTag.set(null);
    }

    async onDeleteTag(id: string){
        const result: Result = await this.tagService.removeAsync(id);

        result.match(
            () => {
                this.tags.update(tags => tags.filter(t => t.id !== id));

                if (this.selectedTag()?.id === id)
                    this.selectedTag.set(null);
            },
            errors => console.error('Ошибка удаления: ', this.mapErrors(errors))
        );
    }

    //#endregion

    //#region Icons

    icons = signal<AssetUrlResponse[]>([]);
    selectedIcon = model<AssetUrlResponse | null>(null);

    // CRUD
    async getIcons() {
        const result: Result<AssetUrlResponse[]> = await this.assetService.getAllAsync();

        result.match(
            assets => this.icons.set(assets),
            errors => console.error('Ошибка получения иконок: ', this.mapErrors(errors))
        );
    }

    //#endregion

    //#region IconCategories

    iconCategories = signal<IconCategoryResponse[]>([]);
    selectedCategory = model<IconCategoryResponse | null>(null);

    // CRUD
    async getIconCategoriesAsync() {
        const result: Result<IconCategoryResponse[]> = await this.iconCategoryService.getAllAsync();

        result.match(
            categories => this.iconCategories.set(categories),
            errors => console.error('Ошибка получения категорий: ', this.mapErrors(errors))
        );
    }

    //#endregion

    //#region Хелперы

    private mapErrors(errors: ErrorList): string{
        return errors.map(e => e.message).join(', ')
    }

    formatDate(value: any): string {
        if (!value) return '';
        
        const date = value.toDate ? value.toDate() : new Date(value);
        
        if (isNaN(date.getTime())) return '';
        
        return date.toLocaleString('ru-RU', {
            day: 'numeric',
            month: 'long',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        });
    }

    openUrl(vault: VaultItemDisplay): void {
         window.open(vault.url, '_blank', 'noopener,noreferrer');
    }

    //#endregion
}