namespace EnigmaVault.Authentication.Client.Model.Responses
{
    public sealed record UserPublicInfo(string ClientSalt, string EncryptedDek);
}