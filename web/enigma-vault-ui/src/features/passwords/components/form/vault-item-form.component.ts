import { DIALOG_DATA, DialogRef } from "@angular/cdk/dialog";
import { CommonModule } from "@angular/common";
import { Component, effect, inject, model, signal } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { CreateVaultItemModalData } from "../../models/modal/create-vault-item.modal-data";
import { VaultTypeEnum } from "../../models/domain/vault-type.enum";
import { VaultItemCommon } from "../../models/modal/vault-item-common.modal";
import { CreateVaultItemResult } from "../../models/modal/create-vault-item.result";

@Component({
    selector: 'create-vault',
    templateUrl: './vault-item-form.component.html',
    styleUrls: ['./vault-item-form.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule
    ]
})
export class CreateVaultComponent {
    private dialogRef = inject(DialogRef<CreateVaultItemResult>);
    private fb = inject(FormBuilder);
    data = inject(DIALOG_DATA) as CreateVaultItemModalData;

    form: FormGroup = this.fb.group({
        // Обязательные
        name: ['', [Validators.required, Validators.minLength(2)]],
        url: ['', []],
        description: ['', []],
        // Обычный пароль
        login: ['', []],
        password: ['', []],
        email: ['', []],
        phone: ['', []],
        secretWord: ['', []],
        recoveryKey: ['', []],
        // Банковская карта
        cardNumber: ['', []],
        cardHolder: ['', []],
        expireDate: ['', []],
        cvvCode: ['', []],
        pinCode: ['', []],
        bankName: ['', []],
        paymentSystem: ['', []],
        // Сервер
        ipAddress: ['', []],
        port: ['', []],
        domain: ['', []],
        serverLogin: ['', []],
        rootPassword: ['', []],
        sshKey: ['', []],
        // Api
        key: ['', []],
        baseUrl: ['', []],
        clientId: ['', []],
        clientSecret: ['', []],
        expirationDate: ['', []],
        environment: ['', []],
        scope: ['', []]
    });

    VaultTypeEnum = VaultTypeEnum;

    activeTab = signal<VaultTypeEnum>(VaultTypeEnum.Password);

    setActiveTab(tab: VaultTypeEnum) {
        this.activeTab.set(tab);
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        const value = this.form.value;
        const common: VaultItemCommon = {
            name: value.name,
            url: value.url,
            description: value.description
        };
        
        let result: CreateVaultItemResult;

        switch (this.activeTab()) {
            case VaultTypeEnum.Password:
                result = {
                    type: VaultTypeEnum.Password,
                    common,
                    details: {
                        Login: value.login,
                        Password: value.password,
                        Email: value.email,
                        Phone: value.phone,
                        SecredWord: value.secretWord,
                        RecoveryKey: value.recoveryKey
                    }
                }
                break;
            case VaultTypeEnum.CreditCard:
                result = {
                    type: VaultTypeEnum.CreditCard,
                    common,
                    details: {
                        CardNumber: value.cardNumber,
                        CardHolder: value.cardHolder,
                        ExpireDate: value.expireDate,
                        CvvCode: value.cvvCode,
                        PinCode: value.pinCode,
                        BankName: value.bankName,
                        PaymentSystem: value.paymentSystem
                    }
                }
                break;
            case VaultTypeEnum.Server:
                result = {
                    type: VaultTypeEnum.Server,
                    common,
                    details: {
                        IpAddress: value.ipAddress,
                        Port: value.port,
                        Domain: value.domain,
                        Login: value.serverLogin,
                        RootPassword: value.rootPassword,
                        SshKey: value.sshKey
                    }
                }
                break;
            case VaultTypeEnum.ApiKey:
                result = {
                    type: VaultTypeEnum.ApiKey,
                    common,
                    details: {
                        Key: value.key,
                        BaseUrl: value.baseUrl,
                        ClientId: value.clientId,
                        ClientSecret: value.clientSecret,
                        ExpirationDate: value.expirationDate,
                        Environment: value.environment,
                        Scope: value.scope
                    }
                }
                break;

            default:
                return;
        }

        this.dialogRef.close(result);
    }

    onCancel(): void {
        this.dialogRef.close();
    }
}