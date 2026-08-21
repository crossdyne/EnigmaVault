namespace Shared.Contracts.PasswordService.Responses
{
    public sealed record FolderResponse(string Id, string UserId, string? ParentFolderId, string FolderName, string Color);
}