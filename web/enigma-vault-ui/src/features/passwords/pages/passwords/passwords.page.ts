import { Component, inject, signal } from "@angular/core";
import { TagService } from "../../services/tag.service";
import { TagResponse } from "../../models/tag.response";
import { ErrorList, Result } from "@crossdyne/toolkit";
import { TagsListComponent } from "../../../../shared/ui/tags/tags-list.component";
import { ItemInputActionsComponent } from "../../../../shared/ui/item-input-actions/item-input-actions.component";
import { CreateTagRequest } from "../../models/create-tag.request";
import { UpdateTagRequest } from "../../models/update-tag.request";

@Component({
    selector: 'passwords-page',
    templateUrl: './passwords.page.html',
    styleUrls: ['./passwords.page.scss'],
    standalone: true,
    imports: [TagsListComponent, ItemInputActionsComponent]
})
export class PasswordsPage {
    private http = inject(TagService);

    selectedTag = signal<TagResponse | null>(null);

    constructor() {
        this.getTagsAsync();
    }
    
    //#region Коллекции

    tags = signal<TagResponse[]>([]);
    editingTag = signal<TagResponse | null>(null);

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

        const result: Result<string> = await this.http.createAsync(request);

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

        const result: Result = await this.http.updateAsync(request);

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
        const result: Result = await this.http.removeAsync(id);

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

    //#endregion

    //#region CRUD

    async getTagsAsync() {
        // const list = 
        //  this.tags.set([
        //     { id: "tag-001", name: "Важно", color: "#EF4444" },
        //     { id: "tag-002", name: "Работа", color: "#3B82F6" },
        //     { id: "tag-003", name: "Личное", color: "#10B981" },
        //     { id: "tag-004", name: "Идеи", color: "#F59E0B" },
        //     { id: "tag-005", name: "Баг", color: "#DC2626" },
        //     { id: "tag-006", name: "Фича", color: "#8B5CF6" },
        //     { id: "tag-007", name: "Документация", color: "#06B6D4" },
        //     { id: "tag-008", name: "Дизайн", color: "#EC4899" },
        //     { id: "tag-009", name: "Тестирование", color: "#84CC16" },
        //     { id: "tag-010", name: "Релиз", color: "#F97316" }
        // ]);
        const result: Result<TagResponse[]> = await this.http.getAllAsync();

        result.match(
            tags => this.tags.set(tags),
            errors => console.log(this.mapErrors(errors))
        );
    } 

    //#endregion

    //#region Хелперы

      private mapErrors(errors: ErrorList): string{
        return errors.map(e => e.message).join(', ')
    }

    //#endregion
}