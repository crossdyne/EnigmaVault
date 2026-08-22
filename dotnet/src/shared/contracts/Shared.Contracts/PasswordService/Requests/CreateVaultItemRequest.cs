namespace Shared.Contracts.PasswordService.Requests
{
    public sealed record CreateVaultItemRequest(int VaultType, string IconId, string EncryptedOverview, string EncryptedDetails, int CryptoVersion);
}