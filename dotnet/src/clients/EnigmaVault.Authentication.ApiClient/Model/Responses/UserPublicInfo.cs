namespace EnigmaVault.Authentication.ApiClient.Model.Responses
{
    public sealed record UserPublicInfo(string ClientSalt, string EncryptedDek);
}