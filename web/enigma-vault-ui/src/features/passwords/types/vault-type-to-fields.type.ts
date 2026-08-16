import { ApiKey } from "../models/domain/api-key";
import { CreditCard } from "../models/domain/credit-card";
import { Server } from "../models/domain/server";
import { StandardPassword } from "../models/domain/standard-password";
import { VaultTypeEnum } from "../models/domain/vault-type.enum";

type VaultTypeToFields = {
    [VaultTypeEnum.Password]: StandardPassword;
    [VaultTypeEnum.Server]: Server;
    [VaultTypeEnum.CreditCard]: CreditCard;
    [VaultTypeEnum.ApiKey]: ApiKey;
};

type VaultField<T extends VaultTypeEnum> = keyof VaultTypeToFields[T] & string;