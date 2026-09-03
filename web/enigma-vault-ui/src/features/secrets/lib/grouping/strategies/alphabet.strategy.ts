import { VaultItemDisplay } from "../../../models/domain/vault-item-display";
import { GroupingVaultsResult } from "../grouping.result";
import { GroupingStrategy } from "../grouping.strategy";

export class AlphabetGroupingStrategy implements GroupingStrategy {
    group(vaults: VaultItemDisplay[]): GroupingVaultsResult[] {
        const groups = new Map<string, VaultItemDisplay[]>();
        for (const vault of vaults) {
            const letter = vault.serviceName[0]?.toUpperCase() || '#';
            if (!groups.has(letter))
                groups.set(letter, []);

            groups.get(letter)!.push(vault);
        }

        return Array.from(groups.entries())
            .sort(([a], [b]) => a.localeCompare(b))
            .map(([title, vaults]) => ({ title, vaults }))
    }
}