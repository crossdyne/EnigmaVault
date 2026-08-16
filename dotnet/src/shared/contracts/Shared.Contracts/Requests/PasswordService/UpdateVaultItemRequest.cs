namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdateVaultItemRequest(string VaultItemId, string IconId, string EncryptedOverview, string EncryptedDetails, int CryptoVersion);
}