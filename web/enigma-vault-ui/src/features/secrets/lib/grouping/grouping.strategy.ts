import { VaultItemDisplay } from "../../models/domain/vault-item-display";
import { GroupingVaultsResult as GroupingResult } from "./grouping.result";

export interface GroupingStrategy {
    group(vaults: VaultItemDisplay[]) : GroupingResult[];
}