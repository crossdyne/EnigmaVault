namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateVaultItemRequest(string PasswordType, string IconId, string EncryptedOverview, string EncryptedDetails);
}