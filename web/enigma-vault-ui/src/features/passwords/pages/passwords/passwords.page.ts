import { Component, effect, inject, model, signal } from "@angular/core";
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

@Component({
    selector: 'passwords-page',
    templateUrl: './passwords.page.html',
    styleUrls: ['./passwords.page.scss'],
    standalone: true,
    imports: [TagsListComponent, ItemInputActionsComponent, ComboboxComponent,]
})
export class PasswordsPage {
    private tagService = inject(TagService);
    private iconCategoryService = inject(IconCategoryService);

    selectedTag = signal<TagResponse | null>(null);

    constructor() {
        this.getTagsAsync();
        this.getIconCategoriesAsync();

        effect(() => {
            const cat = this.selectedCategory();
            
            if (!cat)
                return;

            console.log('Выбранная категория: ', cat.name);
        });
    }
    
    //#region Коллекции

    tags = signal<TagResponse[]>([]);
    iconCategories = signal<IconCategoryResponse[]>([]);

    selectedCategory = model<IconCategoryResponse | null>(null);

    //#endregion

    //#region Сигналы

    activeTab = signal<'tags' | 'icons'>('tags');

    editingTag = signal<TagResponse | null>(null);

    //#endregion

    //#region Get и Set

    getTagName = (tag: TagResponse) => tag.name;
    setTagName = (tag: TagResponse, name: string) => ({ ...tag, name});

    //#endregion

    //#region События

    onSelectedTag(tag: TagResponse) {
        this.selectedTag.set(tag);
    }

    onEditTag(tag: TagResponse) {
        this.editingTag.set(tag);
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

    onCancelEdit() {
        this.editingTag.set(null);
    }

    setActiveTab(tab: 'tags' | 'icons') {
        this.activeTab.set(tab);
    }

    //#endregion

    //#region CRUD

    async getTagsAsync() {
        const result: Result<TagResponse[]> = await this.tagService.getAllAsync();

        result.match(
            tags => this.tags.set(tags),
            errors => console.log(this.mapErrors(errors))
        );
    } 

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

    //#endregion
}