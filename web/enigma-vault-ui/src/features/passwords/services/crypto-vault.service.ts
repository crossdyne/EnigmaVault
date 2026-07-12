import { inject, Injectable } from "@angular/core";
import { CryptoService } from "@crossdyne/security";
import { OverviewPayload } from "../models/domain/overview-payload";
import { CryptoStateService } from "../../../core/services/crypto-state.service";
import { CreditCard } from "../models/domain/credit-card";
import { StandardPassword } from "../models/domain/standard-password";
import { Server } from "../models/domain/server";
import { ApiKey } from "../models/domain/api-key";
import { VaultTypeEnum } from "../models/domain/vault-type.enum";

@Injectable({
    providedIn: 'root'
})
export class VaultCryptoService {
    private state = inject(CryptoStateService);
    private crypto = new CryptoService();
    
    async decryptOverview(encrypted: string): Promise<OverviewPayload | null> {
        return await this.crypto.decryptData<OverviewPayload>(encrypted, this.state.dek!);
    }

    async decryptDetails(type: VaultTypeEnum, encrypted: string) : Promise<StandardPassword | CreditCard | Server | ApiKey | null> {
        switch (type) {
            case VaultTypeEnum.Password:
                return await this.crypto.decryptData<StandardPassword>(encrypted, this.state.dek!);
            case VaultTypeEnum.CreditCard:
                return await this.crypto.decryptData<CreditCard>(encrypted, this.state.dek!);
            case VaultTypeEnum.Server:
                return await this.crypto.decryptData<Server>(encrypted, this.state.dek!);
            case VaultTypeEnum.ApiKey:
                return await this.crypto.decryptData<ApiKey>(encrypted, this.state.dek!);
            default:
                throw new Error(`Неизвестный тип: ${type}`);
        }
    }
    
    async encryptOverview(payload: OverviewPayload): Promise<string> {
        return await this.crypto.encryptData(payload, this.state.dek!);
    }

    async encryptDetails(payload: any): Promise<string> {
        return await this.crypto.encryptData(payload, this.state.dek!);
    }
}