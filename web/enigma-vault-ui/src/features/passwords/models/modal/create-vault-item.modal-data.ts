import { AssetUrlResponse } from "../dto/asset-urls.response";
import { TagResponse } from "../dto/tag.response";

export interface CreateVaultItemModalData {
    assets: AssetUrlResponse[];
    tags: TagResponse[];
}