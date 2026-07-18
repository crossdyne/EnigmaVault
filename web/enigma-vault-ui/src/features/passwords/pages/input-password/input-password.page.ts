import { Component, inject, signal } from "@angular/core";
import { CryptoService, CryptoVersion, KeyDerivationService, SecurityUtils } from "@crossdyne/security";
import { CryptoHttpService } from "../../../../core/services/crypto-http.service";
import { Result } from "@crossdyne/toolkit";
import { DekResponse } from "../../../../core/contracts/crypto/dek.response";
import { CryptoStateService } from "../../../../core/services/crypto-state.service";
import { Router } from "@angular/router";

@Component({
    selector: 'input-password',
    templateUrl: './input-password.page.html',
    styleUrls: ['./input-password.page.scss'],
    standalone: true
})
export class InputPasswordPage {
    private router = inject(Router);
    private cryptoHttp = inject(CryptoHttpService);
    private state = inject(CryptoStateService);

    private keyDerivationService = new KeyDerivationService();
    private cryptoService = new CryptoService();

    constructor() {
        // this.cryptoService.decryptData()
    }

    errors = signal<string>('');
    password = signal<string>('');

    async submit() {
        const result: Result<DekResponse> = await this.cryptoHttp.getDekAsync();
        
        if (result.isFailure){
            this.errors.set(result.stringMessage);
            return;
        }

        const dekResponse = result.value;

        const cryptoVersion: CryptoVersion = dekResponse.cryptoVersion as CryptoVersion;
        const login: string = dekResponse.login.toLowerCase();
        const encryptedDek = dekResponse.encryptedDek;
        const salt: Uint8Array = SecurityUtils.fromBase64(dekResponse.clientSalt);

        let decryptedDek: Uint8Array<ArrayBufferLike> | null = null;

        try {
            const { kek } = await this.keyDerivationService.deriveKeysFromPassword(login, this.password(), salt, cryptoVersion);
            decryptedDek = await this.cryptoService.decryptData<Uint8Array>(encryptedDek, kek, true);
    
        } catch (error) {
            this.errors.set('Вы ввели не верный пароль');
            console.error(error);
            return;
        }

        if (!decryptedDek){
            this.errors.set('Не удалось проверить пароль');
            return;
        }

        this.state.set(decryptedDek, dekResponse.cryptoVersion);
        
        this.router.navigate(['/passwords']);
    }
}