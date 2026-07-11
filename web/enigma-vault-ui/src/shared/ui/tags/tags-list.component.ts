import { Component, input, model, output, signal } from "@angular/core";
import { TagResponse } from "../../../features/passwords/models/dto/tag.response";

@Component({
    selector: 'tags-list',
    templateUrl: './tags-list.component.html',
    styleUrls: ['./tags-list.component.scss'],
    standalone: true
})
export class TagsListComponent {
    tags = input.required<TagResponse[]>();

    selectedTag = output<TagResponse>();
    edit = output<TagResponse>();
    delete = output<string>();
}