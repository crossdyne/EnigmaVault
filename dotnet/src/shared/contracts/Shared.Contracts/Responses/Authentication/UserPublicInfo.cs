namespace Shared.Contracts.Responses.Authentication
{
    public sealed record UserPublicInfo(string ClientSalt, string EncryptedDek);
}