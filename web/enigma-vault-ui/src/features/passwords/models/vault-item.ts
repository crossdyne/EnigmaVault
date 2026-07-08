import { IconUrl } from "./icon-url";
import { TagResponse } from "./tag.response";

export interface VaultItem {
    id: string; 
    serviceName: string;
    url: string;
    type: VaultType;
    dateAdded: Date;
    dateUpdate: Date | null;
    deletedAt: Date | null;
    isFavorite: boolean;
    isArchive: boolean; 
    isInTrash: boolean;
    encryptedOverview: string; 
    encryptedDetails: string, 
    tags: TagResponse[]; 
    icon: IconUrl | undefined;
    note: string | null;
}