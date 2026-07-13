import { DIALOG_DATA, DialogRef } from "@angular/cdk/dialog";
import { Component, inject, signal } from "@angular/core";
import { TagAttachmentResult } from "../../models/modal/tag-attachment.result";
import { TagAttachmentData } from "../../models/modal/tag-attachment.data";

@Component({
    selector: 'attach-tag',
    templateUrl: './attach-tag.component.html',
    styleUrls: ['./attach-tag.component.scss'],
    standalone: true
})
export class AttachTagComponent {
  private dialogRef = inject(DialogRef<TagAttachmentResult>);
  private data = inject(DIALOG_DATA) as TagAttachmentData;

  vault = this.data.vault;
  availableTags = this.data.availableTags;
  
  selectedTagIds = signal<Set<string>>(new Set(this.vault.tags.map(t => t.id)));

  toggleTag(tagId: string) {
    this.selectedTagIds.update(set => {
      const newSet = new Set(set);
      if (newSet.has(tagId)) {
        newSet.delete(tagId);
      } else {
        newSet.add(tagId);
      }
      return newSet;
    });
  }

  isSelected(tagId: string): boolean {
    return this.selectedTagIds().has(tagId);
  }

  onCancel() {
    this.dialogRef.close();
  }

  onSave() {
    this.dialogRef.close({ 
      selectedTagIds: Array.from(this.selectedTagIds()) 
    });
  }
}