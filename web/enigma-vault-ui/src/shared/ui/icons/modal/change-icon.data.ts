import { AssetUrlResponse } from "../../../../features/passwords/models/dto/asset-urls.response";
import { IconCategoryResponse } from "../../../../features/passwords/models/dto/icon-category.response";


export interface ChangeIconData {
    icons: AssetUrlResponse[];
    categories: IconCategoryResponse[];
    serviceName: string;
}