import { ApiKey } from "../domain/api-key";
import { CreditCard } from "../domain/credit-card";
import { OverviewPayload } from "../domain/overview-payload";
import { Server } from "../domain/server";
import { StandardPassword } from "../domain/standard-password";
import { VaultTypeEnum } from "../domain/vault-type.enum";

export interface VaultItemView {
    type: VaultTypeEnum;
    overview: OverviewPayload;
    decryptedDetails: StandardPassword | ApiKey | Server | CreditCard | null;  
}