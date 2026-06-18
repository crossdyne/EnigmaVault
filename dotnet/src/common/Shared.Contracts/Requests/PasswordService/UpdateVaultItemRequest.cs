namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdateVaultItemRequest(string VaultItemId, string EncryptedOverview, string EncryptedDetails);
}