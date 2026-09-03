import { VaultItemDisplay } from "../../../models/domain/vault-item-display";
import { VaultTypeEnum } from "../../../models/domain/vault-type.enum";
import { GroupingVaultsResult } from "../grouping.result";
import { GroupingStrategy } from "../grouping.strategy";

export class TypeGroupingStrategy implements GroupingStrategy {
    group(vaults: VaultItemDisplay[]): GroupingVaultsResult[] {
         const labels: Record<string, string> = {
            [VaultTypeEnum.Password]: 'Пароли',
            [VaultTypeEnum.ApiKey]: 'API ключи',
            [VaultTypeEnum.CreditCard]: 'Банковские карты',
            [VaultTypeEnum.Server]: 'Серверы',
            [VaultTypeEnum.ConnectionString]: 'Строки подключения',
            [VaultTypeEnum.AsymmetricKey]: 'Ассиметричные ключи',
            [VaultTypeEnum.RecoveryKeys]: 'Ключи восстановления',
        };
        
        const groups = new Map<string, VaultItemDisplay[]>();
        for (const vault of vaults) {
            const key = labels[vault.type] ?? 'Другое';
            if (!groups.has(key)) 
                groups.set(key, []);
            
            groups.get(key)!.push(vault);
        }

        return Array.from(groups.entries()).map(([title, vaults]) => ({ title, vaults }));
    }
}