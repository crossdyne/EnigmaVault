import { Component, computed, effect, inject, model, signal, viewChild } from "@angular/core";
import { TagService } from "../../services/tag.service";
import { TagResponse } from "../../models/dto/tag.response";
import { ErrorList, Result } from "@crossdyne/toolkit";
import { CreateTagRequest } from "../../models/dto/create-tag.request";
import { UpdateTagRequest } from "../../models/dto/update-tag.request";
import { IconCategoryResponse } from "../../models/dto/icon-category.response";
import { IconCategoryService } from "../../services/icon-category.service";
import { AssetUrlResponse } from "../../models/dto/asset-urls.response";
import { AssetService } from "../../services/asset.service";
import { EncryptedVaultResponse } from "../../models/dto/encrypted-vault.response";
import { VaultService } from "../../services/vault.service";
import { VaultItemDisplay } from "../../models/domain/vault-item-display";
import { IconUrl } from "../../models/dto/icon-url";
import { OverviewPayload } from "../../models/domain/overview-payload";
import { CryptoStateService } from "../../../../core/services/crypto-state.service";
import { Router } from "@angular/router";
import { ConnectedPosition, OverlayModule } from "@angular/cdk/overlay";
import { CdkContextMenuTrigger, CdkMenu, CdkMenuItem, CdkMenuTrigger } from "@angular/cdk/menu";
import { Dialog } from "@angular/cdk/dialog";
import { VaultItemFormComponent } from "../../components/form/vault-item-form.component";
import { CreateVaultItemRequest } from "../../models/dto/create-vault.request";
import { VaultItemFormData } from "../../models/modal/vault-item-form.data";
import { VaultTypeEnum } from "../../models/domain/vault-type.enum";
import { FormVaultItemResult } from "../../models/modal/form-vault-item.result";
import { VaultCryptoService } from "../../services/crypto-vault.service";
import { UpdateVaultItemRequest } from "../../models/dto/update-vault-item.request";
import { VaultItemViewComponent } from "../../components/view/vault-item-view.component";
import { VaultItemView } from "../../models/modal/vault-item-view";
import { AttachTagComponent } from "../../components/attach-tag/attach-tag.component";
import { TagAttachmentData } from "../../models/modal/tag-attachment.data";
import { TagAttachmentResult } from "../../models/modal/tag-attachment.result";
import { TooltipDirective } from "../../../../shared/directives/tooltip.directive";
import { IconsComponent } from "../../../../shared/ui/icons/icons.component";
import { ChangeIconData } from "../../../../shared/ui/icons/modal/change-icon.data";
import { DateHelper } from "../../../../core/helpers/date.helper";
import { TagsOverflowDirective } from "../../../../shared/directives/tags-overflow.directive";

@Component({
    selector: 'passwords-page',
    templateUrl: './passwords.page.html',
    styleUrls: ['./passwords.page.scss'],
    standalone: true,
    imports: [
        TooltipDirective,
        TagsOverflowDirective,
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

        effect(() => {
            const vault = this.selectedVault();

            if (!vault)
                return;

            console.log('Выбрана запись с именем: ', vault.serviceName);
        })
    }
    
    private async initAsync() {
        await this.getTagsAsync();
        await this.getIconCategoriesAsync();
        await this.getIcons();
        await this.getVaults();
    }

    trigger = viewChild.required<CdkMenuTrigger>('trigger')

    activeTab = signal<'tags' | 'icons'>('tags');
    activeTemplate = signal<'detailed' | 'brief' | 'compact'>('detailed');
    activePopup = signal<'trash' | 'archive' | null>(null);

    //#region Vaults

    vaults = signal<VaultItemDisplay[]>([]);
    activeVaults = computed(() => this.vaults().filter(v => !v.isArchive && !v.isInTrash));
    archivedVaults = computed(() => this.vaults().filter(v => v.isArchive && !v.isInTrash));
    trashedVaults = computed(() => this.vaults().filter(v => v.isInTrash));

    selectedVault = model<VaultItemDisplay | null>(null);

    // Popups
    popupArchivePositions: ConnectedPosition[] = [
        {
            originX: 'center',
            originY: 'bottom',
            overlayX: 'center',
            overlayY: 'top',
            offsetY: 16 
        },
        {
            originX: 'center',
            originY: 'top',
            overlayX: 'center',
            overlayY: 'bottom',
            offsetY: -16
        }
    ];

    popupTrashPositions: ConnectedPosition[] = [
        {
            originX: 'center',
            originY: 'bottom',
            overlayX: 'center',
            overlayY: 'top',
            offsetY: 16 
        },
        {
            originX: 'center',
            originY: 'top',
            overlayX: 'center',
            overlayY: 'bottom',
            offsetY: -16
        }
    ];

    archivePositions = this.popupArchivePositions;
    trashPositions = this.popupTrashPositions;

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
                        type: Number(vault.type) as VaultTypeEnum,
                        serviceName: overview?.ServiceName!,
                        url: overview?.Url!,
                        dateAdded: vault.dateAdded,
                        dateUpdate: vault.dateUpdate,
                        deletedAt: DateHelper.difference(vault.deletedAt),
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
                }
                this.vaults.set(vaultItems);
            },
            async errors => console.error('Ошибка получение паролей: ', this.mapErrors(errors))
        );
    }

    async reloadVaults() {
        this.vaults.set([]);
        await this.getVaults();
    }

    openAddVaultItem() {
        const dialogRef = this.dialog.open<FormVaultItemResult, VaultItemFormData, VaultItemFormComponent>(
            VaultItemFormComponent, { 
                width: '500px',
                disableClose: false,
                hasBackdrop: true,
                backdropClass: 'custom-backdrop',
                data: {
                    mode: 'create'
                }
            }
        );
        
        dialogRef.closed.subscribe(async result => {
            if (!result) 
                return;

            const { encryptedOverView, cryptoVersion: overViewCryptoVersion } = await this.vaultCryptoService.encryptOverview({ 
                ServiceName: result.common.name,
                Url: result.common.url,
                Note: result.common.description
            });

            const { encryptedDetails, cryptoVersion: detailsCryptoVersion } = await this.vaultCryptoService.encryptDetails(result.details);

            if (overViewCryptoVersion != detailsCryptoVersion)
                return;

            const cryptoVersion = overViewCryptoVersion;

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
                    iconId = 'ae71a54a-bea0-428c-a526-62ac9df400dc';
                    break;
            
                default:
                    iconId = '5e3e7328-12b7-4740-ad90-90889e15b58e';
                    break;
            }

            const request: CreateVaultItemRequest = {
                vaultType: result.type as number,
                iconId: iconId,
                encryptedOverview: encryptedOverView,
                encryptedDetails: encryptedDetails,
                cryptoVersion: cryptoVersion
            };

            const resultCreated: Result<string> = await this.vaultService.createAsync(request);

            resultCreated.match(
                async id => {
                    const newVaultResult = await this.vaultService.getByIdAsync(id);

                    newVaultResult.match(
                        async vault => {
                            const decryptedOverView = await this.vaultCryptoService.decryptOverview(vault.encryptedOverview);
                            const fullTags: TagResponse[] = this.tags().filter(t => vault.tagsIds.includes(t.id));
                            
                            const iconUrl = this.icons().find(i => i.assetId === iconId);
                            let icon: IconUrl | undefined = undefined;
                            if (iconUrl)
                                icon = { id: iconUrl?.assetId, url: iconUrl?.url }
                            
                            const display: VaultItemDisplay = {
                                id: vault.id,
                                serviceName: decryptedOverView?.ServiceName!,
                                url: decryptedOverView?.Url!,
                                type: Number(vault.type) as VaultTypeEnum,
                                dateAdded: vault.dateAdded,
                                dateUpdate: vault.dateUpdate,
                                deletedAt: DateHelper.difference(vault.deletedAt),
                                isFavorite: vault.isFavorite,
                                isArchive: vault.isArchive,
                                isInTrash: vault.isInTrash,
                                encryptedOverview: vault.encryptedOverview,
                                encryptedDetails: vault.encryptedDetails,
                                tags: fullTags,
                                icon: icon,
                                note: decryptedOverView?.Note!
                            }

                            this.vaults.update(vaults => [...vaults, display]);
                        },
                        async errors => console.error(this.mapErrors(errors)) 
                    ); 
                },
                async errors => console.error(this.mapErrors(errors))
            );
        });
    }

    async openViewVaultItem(item: VaultItemDisplay) {
        const decryptedDetails = await this.vaultCryptoService.decryptDetails(item.type, item.encryptedDetails);

        this.dialog.open<unknown, VaultItemView, VaultItemViewComponent>(
            VaultItemViewComponent, {
                width: '500px',
                disableClose: false,
                hasBackdrop: true,
                backdropClass: 'custom-backdrop',
                data: {
                    type: item.type,
                    overview: {
                        ServiceName: item.serviceName,
                        Note: item.note!,
                        Url: item.url
                    },
                    decryptedDetails: decryptedDetails
                }
            }
        );
    }

    async openEditVaultItem(item: VaultItemDisplay) {
        const actualItem = this.vaults().find(v => v.id === item.id) ?? item;

        const decryptedDetails = await this.vaultCryptoService.decryptDetails(actualItem.type, actualItem.encryptedDetails);

        const dialogRef = this.dialog.open<FormVaultItemResult, VaultItemFormData, VaultItemFormComponent>(
            VaultItemFormComponent, { 
                width: '500px',
                disableClose: false,
                hasBackdrop: true,
                backdropClass: 'custom-backdrop',
                data: {
                    mode: 'edit',
                    item: actualItem,
                    decryptedDetails: decryptedDetails
                }
            }
        );

        dialogRef.closed.subscribe(async result => {
            if (!result)
                return;

            if (result.id) {
                const { encryptedOverView, cryptoVersion: overViewCryptoVersion } = await this.vaultCryptoService.encryptOverview({
                    ServiceName: result.common.name,
                    Url: result.common.url,
                    Note: result.common.description
                });

                const { encryptedDetails, cryptoVersion: detailsCryptoVersion } = await this.vaultCryptoService.encryptDetails(result.details);

                if (overViewCryptoVersion != detailsCryptoVersion)
                    return;

                const cryptoVersion = overViewCryptoVersion;

                const request: UpdateVaultItemRequest = {
                    vaultItemId: actualItem.id,
                    iconId: actualItem.icon?.id!,
                    encryptedOverview: encryptedOverView,
                    encryptedDetails: encryptedDetails,
                    cryptoVersion: cryptoVersion
                };

                const resultUpdate = await this.vaultService.updateAsync(request);
                
                resultUpdate.match(
                    id => {
                        const now = new Date();

                        this.vaults.update(vaults => 
                            vaults.map(v => v.id === actualItem.id 
                                ? { 
                                    ...v, 
                                    dateUpdate: now,
                                    serviceName: result.common.name,
                                    url: result.common.url!,
                                    note: result.common.description!,
                                    encryptedOverview: encryptedOverView,
                                    encryptedDetails: encryptedDetails,
                                } 
                                : v
                            )
                        );
                        
                        const current = this.selectedVault();
                        if (current?.id === actualItem.id) {
                            this.selectedVault.set({
                                ...current,
                                dateUpdate: now,
                                serviceName: result.common.name,
                                url: result.common.url!,
                                note: result.common.description!,
                                encryptedOverview: encryptedOverView,
                                encryptedDetails: encryptedDetails,
                            });
                        }
                        
                        console.log('Запись успешно обновлена!');
                    },
                    errors => console.error(this.mapErrors(errors))
                );
            }
        });
    }

    async onUpdateVaultIcon() {
        const dialogRef = this.dialog.open<AssetUrlResponse, ChangeIconData, IconsComponent>(
            IconsComponent, { 
                width: '500px',
                disableClose: false,
                hasBackdrop: true,
                backdropClass: 'custom-backdrop',
                data: {
                    icons: this.icons(),
                    categories: this.iconCategories(),
                    serviceName: this.selectedVault()?.serviceName!
                }
            }
        );

        dialogRef.closed.subscribe(async selectedIcon => {
            if (!selectedIcon){
                console.log('Иконка не была выбрана');
                return;
            }
            
            const vault = this.selectedVault();

            if (!vault) {
                console.warn('Сначала выберите запись в списке слева');
                return;
            }

            const result = await this.vaultService.changeIcon(vault.id, selectedIcon.assetId);

            result.match(
                () => {
                    const newIcon: IconUrl = { id: selectedIcon.assetId, url: selectedIcon.url };
                    this.vaults.update(vaults => vaults.map(v => v.id === vault.id ? { ...v, icon: newIcon } : v));
                    this.selectedVault.update(v => v ? { ...v, icon: newIcon } : null);
                },
                errors => console.error('Ошибка смены иконки: ', this.mapErrors(errors))
            );
        });
    }

    // Actions

    async openTagAttachment(vault: VaultItemDisplay) {
        const actualVault = this.vaults().find(v => v.id === vault.id) || vault;

        const dialogRef = this.dialog.open<TagAttachmentResult, TagAttachmentData, AttachTagComponent>(
            AttachTagComponent, {
                width: '500px',
                disableClose: false,
                hasBackdrop: true,
                backdropClass: 'custom-backdrop',
                data: {
                    vault: actualVault,
                    availableTags: this.tags,
                    initialSelectedTagIds: new Set(actualVault.tags.map(t => t.id)),
                    actions: {
                        createTag: async (name: string, color: string) => {
                            const request: CreateTagRequest = { 
                                id: '', 
                                name, 
                                color 
                            };

                            const result = await this.tagService.createAsync(request);
                            let success = false;

                            result.match(
                                id => {
                                    this.tags.update(tags => [{ id, name, color }, ...tags]);
                                    success = true;
                                },
                                errors => console.error('Ошибка создания тега: ', this.mapErrors(errors))
                            );

                            return success;
                        },
                        updateTag: async (id: string, name: string, color: string) => {
                            const request: UpdateTagRequest = { 
                                id, 
                                name, 
                                color
                            };

                            const result = await this.tagService.updateAsync(request);
                            let success = false;

                            result.match(
                                () => {
                                    this.tags.update(tags => tags.map(t => t.id === id ? { ...t, name, color } : t));
                                    
                                    this.vaults.update(vaults => 
                                        vaults.map(v => {
                                            const tagIdx = v.tags.findIndex(t => t.id === id);
                                            
                                            if (tagIdx !== -1) {
                                                const newTags = [...v.tags];
                                                newTags[tagIdx] = { ...newTags[tagIdx], name, color };
                                                return { ...v, tags: newTags };
                                            }
                                            return v;
                                        })
                                    );

                                    const current = this.selectedVault();
                                    if (current) {
                                        const tagIdx = current.tags.findIndex(t => t.id === id);
                                        if (tagIdx !== -1) {
                                            const newTags = [...current.tags];
                                            newTags[tagIdx] = { ...newTags[tagIdx], name, color };
                                            this.selectedVault.set({ ...current, tags: newTags });
                                        }
                                    }
                                    success = true;
                                },
                                errors => console.error('Ошибка обновления тега: ', this.mapErrors(errors))
                            );
                            return success;
                        },
                        deleteTag: async (id: string) => {
                            const result = await this.tagService.removeAsync(id);
                            let success = false;
                            result.match(
                                () => {
                                    this.tags.update(tags => tags.filter(t => t.id !== id));
                                    
                                    this.vaults.update(vaults => 
                                        vaults.map(v => {
                                            const newTags = v.tags.filter(t => t.id !== id);
                                            if (newTags.length !== v.tags.length) {
                                                return { ...v, tags: newTags };
                                            }
                                            return v;
                                        })
                                    );

                                    const current = this.selectedVault();
                                    if (current) {
                                        const newTags = current.tags.filter(t => t.id !== id);
                                        if (newTags.length !== current.tags.length) {
                                            this.selectedVault.set({ ...current, tags: newTags });
                                        }
                                    }
                                    success = true;
                                },
                                errors => console.error('Ошибка удаления тега: ', this.mapErrors(errors))
                            );
                            return success;
                        }
                    }
                }
            }
        );

        dialogRef.closed.subscribe(async result => {
            if (!result) 
                return;

            if (result.tagsModified) {
                const selectedTagIds = result.selectedTagIds;
                
                const updateResult = await this.vaultService.updateTagsAsync(actualVault.id, { tagIds: selectedTagIds });
                updateResult.match(
                    () => {
                        const updatedTags = this.tags().filter(t => selectedTagIds.includes(t.id));

                        this.vaults.update(vaults => 
                            vaults.map(v => v.id === actualVault.id ? { ...v, tags: updatedTags } : v)
                        );

                        const current = this.selectedVault();
                        if (current?.id === actualVault.id) {
                            this.selectedVault.set({ ...current, tags: updatedTags });
                        }

                        console.log('Тэги успешно обновлены для записи');
                    },
                    errors => console.error('Ошибка обновления тэгов записи: ', this.mapErrors(errors))
                );
            }
        });
    }

    async onMoveToArchive(vault: VaultItemDisplay) {
        const result: Result = await this.vaultService.zipAsync(vault.id);

        result.match(
            () => {
                this.vaults.update(vaults => vaults.map(v => v.id === vault.id ? { ...v, isArchive: true, isInTrash: false } : v ));

                if (this.selectedVault()?.id === vault.id){
                    this.selectedVault.set(null);
                }
            },
            errors => console.error(this.mapErrors(errors))
        );
    }

    async onRestoreFromArchive(vault: VaultItemDisplay){
        const result: Result = await this.vaultService.unZipAsync(vault.id);

        result.match(
            () => this.vaults.update(vaults => vaults.map(v => v.id === vault.id ? { ...v, isArchive: false } : v)),
            errors => console.error(this.mapErrors(errors))
        );
    }

    async unZipAllAsync() { 
        const result: Result = await this.vaultService.unZipAllAsync(); 

        result.match(
            () => this.vaults.update(vaults => vaults.map(v => v.isArchive ? { ...v, isArchive: false } : v)),
            errors => console.error(this.mapErrors(errors))
        );
    }

    async onMoveToTrash(vault: VaultItemDisplay) {
        const result: Result<Date> = await this.vaultService.moveToTrashAsync(vault.id);

        result.match(
            date => {         
                this.vaults.update(vaults => vaults.map(v => v.id === vault.id ? { ...v, isInTrash: true, deletedAt: DateHelper.difference(date) } : v));

                if (this.selectedVault()?.id === vault.id) {
                    this.selectedVault.set(null);
                }
            },
            errors => console.error(this.mapErrors(errors))
        );
    }

    async onRestoreFromTrash(vault: VaultItemDisplay) {
        const result: Result = await this.vaultService.restoreFromTrashAsync(vault.id);

        result.match(
            () => this.vaults.update(vaults => vaults.map(v => v.id === vault.id ? { ...v, isInTrash: false } : v)),
            errors => console.error(this.mapErrors(errors))
        );
    }

    async restoreAllAsync() {
        const result: Result = await this.vaultService.restoreAllFromTrashAsync(); 

        result.match(
            () => this.vaults.update(vaults => vaults.map(v => v.isInTrash ? { ...v, isInTrash: false } : v)),
            errors => console.error(this.mapErrors(errors))
        );
    }
    
    async onDelete(vault: VaultItemDisplay) {
        const result: Result = await this.vaultService.removeAsync(vault.id);

        result.match(
            () => this.vaults.update(vaults => vaults.filter(v => v.id !== vault.id)),
            errors => console.error(this.mapErrors(errors))
        );
    }

    async emptyTrashAsync() {
         const result: Result = await this.vaultService.emptyTrashAsync(); 

        result.match(
            () => this.vaults.update(vaults => vaults.filter(v => !v.isInTrash)),
            errors => console.error(this.mapErrors(errors))
        );
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
  
    //#region Группировка \ Сортировка Vaults

    groupBy = signal<'none' | 'alphabet' | 'type' | 'date' | 'history' | 'tags'>('none');
    sortBy = signal<'none' | 'ascending' | 'descending'>('ascending');
    
    groupedVaults = computed(() => {
        const vaults = this.activeVaults();
        const group = this.groupBy();
        const sort = this.sortBy();

        const applySort = (items: VaultItemDisplay[]) => {
            if (sort === 'none')
                return items;

            return [...items].sort((a, b) => {
                const nameA = a.serviceName.toLowerCase();
                const nameB = b.serviceName.toLowerCase();
                return sort === 'ascending' ? nameA.localeCompare(nameB) : nameB.localeCompare(nameA);
            });
        };

        let result: { title: string; vaults: VaultItemDisplay[] }[];

        if (group === 'none') {
            result = [{ title: 'Все записи', vaults }];
        } else if (group === 'alphabet') {
            const groups = new Map<string, VaultItemDisplay[]>();
            for (const vault of vaults) {
                const letter = vault.serviceName[0]?.toUpperCase() || '#';
                if (!groups.has(letter))
                    groups.set(letter, []);

                groups.get(letter)!.push(vault);
            }

            result = Array.from(groups.entries())
                .sort(([a], [b]) => a.localeCompare(b))
                .map(([title, vaults]) => ({ title, vaults }));
        } else if (group === 'type') {
            const labels: Record<string, string> = {
                [VaultTypeEnum.Password]: 'Пароли',
                [VaultTypeEnum.ApiKey]: 'API ключи',
                [VaultTypeEnum.CreditCard]: 'Банковские карты',
                [VaultTypeEnum.Server]: 'Серверы',
            };

            const groups = new Map<string, VaultItemDisplay[]>();
            for (const vault of vaults) {
                const key = labels[vault.type] ?? 'Другое';
                if (!groups.has(key)) 
                    groups.set(key, []);
                
                groups.get(key)!.push(vault);
            }

            result = Array.from(groups.entries()).map(([title, vaults]) => ({ title, vaults }));
        } else if (group === 'history') {
            const updatedVaults = vaults.filter(vault => {
                if (!vault.dateUpdate) 
                    return false;
                
                const added = vault.dateAdded instanceof Date ? vault.dateAdded : new Date(vault.dateAdded ?? 0);
                const updated = vault.dateUpdate instanceof Date ? vault.dateUpdate : new Date(vault.dateUpdate);
                
                return Math.abs(updated.getTime() - added.getTime()) > 60000;
            });

            if (updatedVaults.length === 0) {
                result = [{ title: 'Нет изменённых записей', vaults: [] }];
            } else {
                const now = new Date();
                const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                const yesterday = new Date(today);
                yesterday.setDate(yesterday.getDate() - 1);
                const weekAgo = new Date(today);
                weekAgo.setDate(weekAgo.getDate() - 7);

                const groups: Record<string, VaultItemDisplay[]> = {
                    'Изменены сегодня': [],
                    'Изменены вчера': [],
                    'Изменены на этой неделе': [],
                    'Изменены ранее': []
                };

                for (const vault of updatedVaults) {
                    const updated = vault.dateUpdate instanceof Date ? vault.dateUpdate : new Date(vault.dateUpdate!);
                    const updStart = new Date(updated.getFullYear(), updated.getMonth(), updated.getDate());

                    if (updStart.getTime() === today.getTime()) {
                        groups['Изменены сегодня'].push(vault);
                    } else if (updStart.getTime() === yesterday.getTime()) {
                        groups['Изменены вчера'].push(vault);
                    } else if (updated >= weekAgo) {
                        groups['Изменены на этой неделе'].push(vault);
                    } else {
                        groups['Изменены ранее'].push(vault);
                    }
                }

                const sortDesc = (a: VaultItemDisplay, b: VaultItemDisplay) => {
                    const da = a.dateUpdate instanceof Date ? a.dateUpdate : new Date(a.dateUpdate!);
                    const db = b.dateUpdate instanceof Date ? b.dateUpdate : new Date(b.dateUpdate!);
                    return db.getTime() - da.getTime();
                };

                result = Object.entries(groups)
                    .filter(([, vaults]) => vaults.length > 0)
                    .map(([title, vaults]) => ({ title, vaults: vaults.sort(sortDesc) }));
            }
        } else if (group === 'date') {
            const now = new Date();
            const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            const todayTime = today.getTime();
            
            const yesterday = new Date(today);
            yesterday.setDate(yesterday.getDate() - 1);
            const yesterdayTime = yesterday.getTime();
            
            const weekAgo = new Date(today);
            weekAgo.setDate(weekAgo.getDate() - 7);
            const weekAgoTime = weekAgo.getTime();

            const groups: Record<string, VaultItemDisplay[]> = {
                'Сегодня': [],
                'Вчера': [],
                'На этой неделе': [],
                'В этом месяце': [],
                'Ранее': []
            };

            for (const vault of vaults) {
                const d = vault.dateAdded instanceof Date ? vault.dateAdded : new Date(vault.dateAdded);
                
                if (isNaN(d.getTime())) {
                    groups['Ранее'].push(vault);
                    continue;
                }

                const dStart = new Date(d.getFullYear(), d.getMonth(), d.getDate());
                const dTime = dStart.getTime();

                if (dTime === todayTime) {
                    groups['Сегодня'].push(vault);
                } else if (dTime === yesterdayTime) {
                    groups['Вчера'].push(vault);
                } else if (dTime >= weekAgoTime) {
                    groups['На этой неделе'].push(vault);
                } else if (d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear()) {
                    groups['В этом месяце'].push(vault);
                } else {
                    groups['Ранее'].push(vault);
                }
            }

            result = Object.entries(groups)
                .filter(([, vaults]) => vaults.length > 0)
                .map(([title, vaults]) => ({ title, vaults }));
        } else if (group === 'tags') {
            const groups = new Map<string, VaultItemDisplay[]>();
            const untagged: VaultItemDisplay[] = [];

            for (const vault of vaults) {
                if (!vault.tags || vault.tags.length === 0) {
                    untagged.push(vault);
                    continue;
                }
                for (const tag of vault.tags) {
                    const name = tag.name;
                    if (!groups.has(name)) 
                        groups.set(name, []);

                    groups.get(name)!.push(vault);
                }
            }

            result = Array.from(groups.entries())
                .sort(([a], [b]) => a.localeCompare(b))
                .map(([title, vaults]) => ({ title, vaults }));

            if (untagged.length > 0) {
                result.push({ title: 'Без тегов', vaults: untagged });
            }
        } else {
            result = [{ title: 'Все записи', vaults }];
        }

        return result.map(g => ({ ...g, vaults: applySort(g.vaults) }));
    });

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