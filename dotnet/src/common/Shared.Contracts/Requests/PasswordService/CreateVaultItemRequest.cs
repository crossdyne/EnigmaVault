namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateVaultItemRequest(string PasswordType, string EncryptedOverview, string EncryptedDetails);
}