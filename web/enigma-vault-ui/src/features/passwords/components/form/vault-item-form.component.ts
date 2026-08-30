import { DIALOG_DATA, DialogRef } from "@angular/cdk/dialog";
import { CommonModule } from "@angular/common";
import { Component, computed, inject, signal } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { VaultItemFormData } from "../../models/modal/vault-item-form.data";
import { VaultTypeEnum } from "../../models/domain/vault-type.enum";
import { VaultItemCommon } from "../../models/modal/vault-item-common.modal";
import { FormVaultItemResult } from "../../models/modal/form-vault-item.result";
import { VaultItemDisplay } from "../../models/domain/vault-item-display";

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
export class VaultItemFormComponent {
    private dialogRef = inject(DialogRef<FormVaultItemResult>);
    private fb = inject(FormBuilder);
    data = inject(DIALOG_DATA) as VaultItemFormData;

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
        scope: ['', []],
        // ConnectionString
        connectionStringValue: ['', []],
        connectionStringApplication: ['', []],
        // AsymmetricKey
        asymmetricPublicKey: ['', []],
        asymmetricPrivateKey: ['', []],
        asymmetricKeyApplication: ['', []],
        algorithm: ['', []],
        keySize: ['', []],
        passphrase: ['', []],
        format: ['', []],
        fingerprint: ['', []],
        asymmetricKeyDateCreation: ['', []],
        asymmetricKeyDateExpire: ['', []],
    });

    VaultTypeEnum = VaultTypeEnum;

    vaultTypeEnToRu: Record<VaultTypeEnum, string> = {
        1: "Пароля",
        2: "Кредитной карты",
        3: "Доступа к серверу",
        4: "Апи ключа",
        5: "Строки подключения",
        6: "Ассиметричных ключей",
    } 

    activeTab = signal<VaultTypeEnum>(VaultTypeEnum.Password);
    title = computed(() => this.vaultTypeEnToRu[this.activeTab()]);
    fullTitle = computed(() => `${this.isEdit() ? 'Редактирование' : 'Создание'} ${this.title()}`);

    readonly isEdit = signal(false);
    readonly vault = signal<VaultItemDisplay | null>(null);

    constructor() {
        if (this.data.mode === 'edit' && this.data.item) {
            this.isEdit.set(true);
            this.activeTab.set(this.data.item.type);

            this.form.patchValue({
                name: this.data.item.serviceName,
                url: this.data.item.url ?? '',
                description: this.data.item.note ?? '',
            });

            if (this.data.decryptedDetails) {
                const d = this.data.decryptedDetails;
                const patch: Record<string, any> = {};

                switch (this.data.item.type) {
                    case VaultTypeEnum.Password:
                        patch['login'] = d['Login'];
                        patch['password'] = d['Password'];
                        patch['email'] = d['Email'];
                        patch['phone'] = d['Phone'];
                        patch['secretWord'] = d['SecretWord']; 
                        patch['recoveryKey'] = d['RecoveryKey'];
                        break;

                    case VaultTypeEnum.CreditCard:
                        patch['cardNumber'] = d['CardNumber'];
                        patch['cardHolder'] = d['CardHolder'];
                        patch['expireDate'] = d['ExpireDate'];
                        patch['cvvCode'] = d['CvvCode'];
                        patch['pinCode'] = d['PinCode'];
                        patch['bankName'] = d['BankName'];
                        patch['paymentSystem'] = d['PaymentSystem'];
                        break;

                    case VaultTypeEnum.Server:
                        patch['ipAddress'] = d['IpAddress'];
                        patch['port'] = d['Port'];
                        patch['domain'] = d['Domain'];
                        patch['serverLogin'] = d['Login']; 
                        patch['rootPassword'] = d['RootPassword'];
                        patch['sshKey'] = d['SshKey'];
                        break;

                    case VaultTypeEnum.ApiKey:
                        patch['key'] = d['Key'];
                        patch['baseUrl'] = d['BaseUrl'];
                        patch['clientId'] = d['ClientId'];
                        patch['clientSecret'] = d['ClientSecret'];
                        patch['expirationDate'] = d['ExpirationDate'];
                        patch['environment'] = d['Environment'];
                        patch['scope'] = d['Scope'];
                        break;
                        
                    case VaultTypeEnum.ConnectionString:
                        patch['connectionStringValue'] = d['Value'];
                        patch['connectionStringApplication'] = d['Application'];
                        break;

                    case VaultTypeEnum.AsymmetricKey:
                        patch['asymmetricPublicKey'] = d['PublicKey'];
                        patch['asymmetricPrivateKey'] = d['PrivateKey'];
                        patch['asymmetricKeyApplication'] = d['Application'];
                        patch['algorithm'] = d['Algorithm'];
                        patch['keySize'] = d['KeySize'];
                        patch['passphrase'] = d['Passphrase'];
                        patch['format'] = d['Format'];
                        patch['fingerprint'] = d['Fingerprint'];
                        patch['asymmetricKeyDateCreation'] = d['DateCreation'];
                        patch['asymmetricKeyDateExpire'] = d['DateExpire'];
                        break;
                }

                this.form.patchValue(patch);
            }
        }
    }

    setActiveTab(tab: VaultTypeEnum) {
        if (this.isEdit()) {
            return;
        }

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
        
        let result: FormVaultItemResult;

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
                        SecretWord: value.secretWord,
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
                case VaultTypeEnum.ConnectionString:
                result = {
                    type: VaultTypeEnum.ConnectionString,
                    common,
                    details: {
                        Value: value.connectionStringValue,
                        Application: value.connectionStringApplication,
                    }
                }
                break;
                case VaultTypeEnum.AsymmetricKey:
                result = {
                    type: VaultTypeEnum.AsymmetricKey,
                    common,
                    details: {
                        PublicKey: value.asymmetricPublicKey,
                        PrivateKey: value.asymmetricPrivateKey,
                        Application: value.asymmetricKeyApplication,
                        Algorithm: value.algorithm,
                        KeySize: value.keySize,
                        Passphrase: value.passphrase,
                        Format: value.format,
                        Fingerprint: value.fingerprint,
                        DateCreation: value.asymmetricKeyDateCreation,
                        DateExpire: value.asymmetricKeyDateExpire,
                    }
                }
                break;

            default:
                return;
        }

        if (this.isEdit() && this.data.item?.id) {
            result = { ...result, id: this.data.item.id}
        }

        this.dialogRef.close(result);
    }

    onCancel(): void {
        this.dialogRef.close();
    }
}