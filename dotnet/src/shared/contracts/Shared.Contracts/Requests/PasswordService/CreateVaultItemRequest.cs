namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateVaultItemRequest(int VaultType, string IconId, string EncryptedOverview, string EncryptedDetails, int CryptoVersion);
}