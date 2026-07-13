import { VaultItemDisplay } from "../domain/vault-item-display";
import { TagResponse } from "../dto/tag.response";

export interface TagAttachmentData {
  vault: VaultItemDisplay;
  availableTags: TagResponse[];
}