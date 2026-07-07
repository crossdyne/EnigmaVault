import { Component, effect, input, output, signal } from '@angular/core';
import { TagResponse } from '../../../features/passwords/models/tag.response';

@Component({
  selector: 'item-input-actions',
  imports: [],
  templateUrl: './item-input-actions.component.html',
  styleUrl: './item-input-actions.component.scss',
})
export class ItemInputActionsComponent {
  editing = input<TagResponse | null>(null);

  tagName = signal<string>('');
  
  cancelEdit = output<void>();
  update = output<TagResponse>();
  create = output<string>();

  constructor() {
    effect(() => {
      const tag = this.editing();
      this.tagName.set(tag ? tag.name : '');
    });
  }
    
  onAdd() {
    const value = this.tagName().trim();

    if (value) {
      this.create.emit(value);
      this.tagName.set('');
    }
  }

  onSave() {
    const tag = this.editing();
    if (tag) {
      this.update.emit({ ...tag, name: this.tagName() });
    }
  }

  onCancel() {
    this.cancelEdit.emit();
  }
}