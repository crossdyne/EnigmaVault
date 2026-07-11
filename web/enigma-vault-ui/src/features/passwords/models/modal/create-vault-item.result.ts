import { ApiKey } from "../domain/api-key";
import { CreditCard } from "../domain/credit-card";
import { Server } from "../domain/server";
import { StandardPassword } from "../domain/standard-password";
import { VaultTypeEnum } from "../domain/vault-type.enum";
import { VaultItemCommon } from "./vault-item-common.modal";

export type CreateVaultItemResult =
  | { type: VaultTypeEnum.Password;   common: VaultItemCommon; details: StandardPassword }
  | { type: VaultTypeEnum.CreditCard;  common: VaultItemCommon; details: CreditCard }
  | { type: VaultTypeEnum.Server;     common: VaultItemCommon; details: Server }
  | { type: VaultTypeEnum.ApiKey;     common: VaultItemCommon; details: ApiKey };