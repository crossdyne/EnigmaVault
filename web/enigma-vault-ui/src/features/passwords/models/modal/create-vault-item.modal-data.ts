import { AssetUrlResponse } from "../asset-urls.response";
import { TagResponse } from "../tag.response";

export interface CreateVaultItemModalData {
    assets: AssetUrlResponse[];
    tags: TagResponse[];
}