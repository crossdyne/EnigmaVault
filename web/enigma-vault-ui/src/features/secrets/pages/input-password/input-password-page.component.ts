import { Component, inject, signal } from "@angular/core";
import { CryptoHttpService } from "../../../../core/services/crypto-http.service";
import { CryptoWorkerService } from "../../../../core/services/crypto-worker.service";
import { Result } from "@crossdyne/toolkit";
import { DekResponse } from "../../../../core/contracts/crypto/dek.response";
import { Router } from "@angular/router";

@Component({
    selector: 'input-password',
    templateUrl: './input-password-page.component.html',
    styleUrls: ['./input-password-page.component.scss'],
    standalone: true
})
export class InputPasswordPageComponent {
    private router = inject(Router);
    private cryptoHttp = inject(CryptoHttpService);
    private cryptoWorker = inject(CryptoWorkerService);

    errors = signal<string>('');
    password = signal<string>('');

    async submit() {
        const result: Result<DekResponse> = await this.cryptoHttp.getDekAsync();
        
        if (result.isFailure) {
            this.errors.set(result.stringMessage);
            return;
        }

        const dekResponse = result.value;

        try {
            await this.cryptoWorker.init(
                dekResponse.login,
                this.password(),
                dekResponse.clientSalt,
                dekResponse.encryptedDek,
                dekResponse.cryptoVersion as any
            );
        } catch (error) {
            this.errors.set('Вы ввели не верный пароль');
            console.error(error);
            return;
        }

        this.password.set('');

        this.router.navigate(['/passwords']);
    }
}