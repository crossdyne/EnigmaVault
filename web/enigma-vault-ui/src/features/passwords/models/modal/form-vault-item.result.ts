import { ApiKey } from "../domain/api-key";
import { CreditCard } from "../domain/credit-card";
import { Server } from "../domain/server";
import { StandardPassword } from "../domain/standard-password";
import { VaultTypeEnum } from "../domain/vault-type.enum";
import { VaultItemCommon } from "./vault-item-common.modal";

export interface FormVaultItemBase {
    id?: string;
    type: VaultTypeEnum;
    common: VaultItemCommon;
}

export type FormVaultItemResult =
  | (FormVaultItemBase & { type: VaultTypeEnum.Password;    details: StandardPassword })
  | (FormVaultItemBase & { type: VaultTypeEnum.CreditCard;  details: CreditCard })
  | (FormVaultItemBase & { type: VaultTypeEnum.Server;      details: Server })
  | (FormVaultItemBase & { type: VaultTypeEnum.ApiKey;      details: ApiKey });