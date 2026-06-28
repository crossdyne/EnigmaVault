namespace Shared.Contracts.Responses.PasswordService
{
    public sealed record FolderResponse(string Id, string UserId, string? ParentFolderId, string FolderName, string Color);
}