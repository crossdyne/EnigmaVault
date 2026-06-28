namespace Shared.Contracts.Responses.PasswordService
{
    public sealed record EncryptedVaultResponse(string Id, string Type, DateTime DateAdded, DateTime? DateUpdate, DateTime? DeletedAt, bool IsFavorite, bool IsArchive, bool IsInTrash, byte[] EncryptedOverview, byte[] EncryptedDetails, List<string> TagsIds, string IconId);
}