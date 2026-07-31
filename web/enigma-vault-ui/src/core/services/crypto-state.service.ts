import { Injectable } from "@angular/core";
import { CryptoVersion } from "@crossdyne/security";

@Injectable({
    providedIn: 'root'
})
export class CryptoStateService {
    dek: Uint8Array<ArrayBufferLike> | null = null;
    cryptoVersion: CryptoVersion | null = null;

    set(dek: Uint8Array<ArrayBufferLike>, version: number) {
        this.dek = dek;
        this.cryptoVersion = version as CryptoVersion;
    }

    clear() {
        this.dek = null;
    }
}