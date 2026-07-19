import { Component, computed, inject, signal } from "@angular/core";
import { VaultTypeEnum } from "../../models/domain/vault-type.enum";
import { DIALOG_DATA, DialogRef } from "@angular/cdk/dialog";
import { OverviewPayload } from "../../models/domain/overview-payload";
import { VaultItemView } from "../../models/modal/vault-item-view";
import { StandardPassword } from "../../models/domain/standard-password";
import { ApiKey } from "../../models/domain/api-key";
import { Server } from "../../models/domain/server";
import { CreditCard } from "../../models/domain/credit-card";
import { CopyButton } from "../../../../shared/ui/copy-button/copy-button";

@Component({
    selector: 'vault-item-view',
    templateUrl: './vault-item-view.component.html',
    styleUrls: ['./vault-item-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ]
})
export class VaultItemViewComponent {
    private dialogRef = inject(DialogRef);

    vaultTypeEnToRu: Record<VaultTypeEnum, string> = {
        1: "Пароль",
        2: "Кредитная карта",
        3: "Доступ к серверу",
        4: "Апи ключ"
    } 

    data = inject(DIALOG_DATA) as VaultItemView;
    VaultTypeEnum = VaultTypeEnum;
    
    type = signal<VaultTypeEnum | undefined>(undefined);
    typeRu = computed(() => this.vaultTypeEnToRu[this.type()!]);
    overview = signal<OverviewPayload | null>(null);
    decryptedDetails = signal<StandardPassword | ApiKey | Server | CreditCard | null>(null);

    passwordDetails = computed<StandardPassword | null>(() => this.type() === VaultTypeEnum.Password ? (this.decryptedDetails() as StandardPassword) : null);
    creditCardDetails = computed<CreditCard | null>(() => this.type() === VaultTypeEnum.CreditCard ? (this.decryptedDetails() as CreditCard) : null);
    serverDetails = computed<Server | null>(() => this.type() === VaultTypeEnum.Server ? (this.decryptedDetails() as Server) : null);
    apiKeyDetails = computed<ApiKey | null>(() => this.type() === VaultTypeEnum.ApiKey ? (this.decryptedDetails() as ApiKey) : null);

    constructor() {
        this.type.set(this.data.type)
        this.overview.set(this.data.overview);
        this.decryptedDetails.set(this.data.decryptedDetails)
    }

    onCancel(): void {
        this.dialogRef.close();
    }
}