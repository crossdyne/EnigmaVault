import { VaultItemDisplay } from "../../../models/domain/vault-item-display";
import { GroupingVaultsResult } from "../grouping.result";
import { GroupingStrategy } from "../grouping.strategy";

export class HistoryGroupingStrategy implements GroupingStrategy {
    group(vaults: VaultItemDisplay[]): GroupingVaultsResult[] {
        let result: GroupingVaultsResult[];

        const updatedVaults = vaults.filter(vault => {
            if (!vault.dateUpdate) 
                return false;
            
            const added = vault.dateAdded instanceof Date ? vault.dateAdded : new Date(vault.dateAdded ?? 0);
            const updated = vault.dateUpdate instanceof Date ? vault.dateUpdate : new Date(vault.dateUpdate);
            
            return Math.abs(updated.getTime() - added.getTime()) > 60000;
        });
    
        if (updatedVaults.length === 0) {
            result = [{ title: 'Нет изменённых записей', vaults: [] }];
        } else {
            const now = new Date();
            const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            const yesterday = new Date(today);
            yesterday.setDate(yesterday.getDate() - 1);
            const weekAgo = new Date(today);
            weekAgo.setDate(weekAgo.getDate() - 7);

            const groups: Record<string, VaultItemDisplay[]> = {
                'Изменены сегодня': [],
                'Изменены вчера': [],
                'Изменены на этой неделе': [],
                'Изменены ранее': []
            };

            for (const vault of updatedVaults) {
                const updated = vault.dateUpdate instanceof Date ? vault.dateUpdate : new Date(vault.dateUpdate!);
                const updStart = new Date(updated.getFullYear(), updated.getMonth(), updated.getDate());

                if (updStart.getTime() === today.getTime()) {
                    groups['Изменены сегодня'].push(vault);
                } else if (updStart.getTime() === yesterday.getTime()) {
                    groups['Изменены вчера'].push(vault);
                } else if (updated >= weekAgo) {
                    groups['Изменены на этой неделе'].push(vault);
                } else {
                    groups['Изменены ранее'].push(vault);
                }
            }

            const sortDesc = (a: VaultItemDisplay, b: VaultItemDisplay) => {
                const da = a.dateUpdate instanceof Date ? a.dateUpdate : new Date(a.dateUpdate!);
                const db = b.dateUpdate instanceof Date ? b.dateUpdate : new Date(b.dateUpdate!);
                return db.getTime() - da.getTime();
            };

            result = Object.entries(groups)
                .filter(([, vaults]) => vaults.length > 0)
                .map(([title, vaults]) => ({ title, vaults: vaults.sort(sortDesc) }));
        }

        return result;
    }
}