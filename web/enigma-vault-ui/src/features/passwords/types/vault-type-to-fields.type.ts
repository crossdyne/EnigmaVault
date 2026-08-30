import { ApiKey } from "../models/domain/api-key";
import { AsymmetricKey } from "../models/domain/asymmetric-key";
import { ConnectionString } from "../models/domain/connection-string";
import { CreditCard } from "../models/domain/credit-card";
import { Server } from "../models/domain/server";
import { StandardPassword } from "../models/domain/standard-password";
import { VaultTypeEnum } from "../models/domain/vault-type.enum";

type VaultTypeToFields = {
    [VaultTypeEnum.Password]: StandardPassword;
    [VaultTypeEnum.Server]: Server;
    [VaultTypeEnum.CreditCard]: CreditCard;
    [VaultTypeEnum.ApiKey]: ApiKey;
    [VaultTypeEnum.ConnectionString]: ConnectionString;
    [VaultTypeEnum.AsymmetricKey]: AsymmetricKey;
};

type VaultField<T extends VaultTypeEnum> = keyof VaultTypeToFields[T] & string;