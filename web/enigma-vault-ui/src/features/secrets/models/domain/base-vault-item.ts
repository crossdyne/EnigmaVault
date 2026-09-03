import { ApiKey } from "./api-key";
import { CreditCard } from "./credit-card";
import { OverviewPayload } from "./overview-payload";
import { Server } from "./server";
import { StandardPassword } from "./standard-password";
import { VaultTypeEnum } from "./vault-type.enum";

interface BaseVaultItem {
    id: string;
    isReadOnly: boolean;
    isArchive: boolean;
    isInTrash: boolean;
    overview: OverviewPayload;
}

export type VaultItem =
    | BaseVaultItem & { type: VaultTypeEnum.CreditCard; data: CreditCard }
    | BaseVaultItem & { type: VaultTypeEnum.Password; data: StandardPassword }
    | BaseVaultItem & { type: VaultTypeEnum.Server; data: Server }
    | BaseVaultItem & { type: VaultTypeEnum.ApiKey; data: ApiKey };