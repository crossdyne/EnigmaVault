import { Injectable } from "@angular/core";
import { HttpService } from "../../../core/http/http.service";
import { Result } from "@crossdyne/toolkit";
import { EncryptedVaultResponse } from "../models/dto/encrypted-vault.response";
import { CreateVaultItemRequest } from "../models/dto/create-vault.request";

@Injectable({
    providedIn: 'root'
})
export class VaultService extends HttpService {

    constructor() {
        super('api/v1/vault')
    }

    async createAsync(request: CreateVaultItemRequest): Promise<Result<string>> {
        return await this.postAsync('', request);
    }

    async getAllAsync(): Promise<Result<EncryptedVaultResponse[]>> {
        return this.getAsync('');
    }
}