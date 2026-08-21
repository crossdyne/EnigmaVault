namespace Shared.Contracts.PasswordService.Requests
{
    public sealed record UpdateVaultItemRequest(string VaultItemId, string IconId, string EncryptedOverview, string EncryptedDetails, int CryptoVersion);
}