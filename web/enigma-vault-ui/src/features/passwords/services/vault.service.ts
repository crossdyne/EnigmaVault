import { Injectable } from "@angular/core";
import { HttpService } from "../../../core/http/http.service";
import { Result } from "@crossdyne/toolkit";
import { EncryptedVaultResponse } from "../models/encrypted-vault.response";

@Injectable({
    providedIn: 'root'
})
export class VaultService extends HttpService {

    constructor() {
        super('api/v1/vault')
    }

    async getAllAsync(): Promise<Result<EncryptedVaultResponse[]>> {
        return this.getAsync('');
    }
}