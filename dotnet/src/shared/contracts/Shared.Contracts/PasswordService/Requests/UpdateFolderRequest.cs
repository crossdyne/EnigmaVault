namespace Shared.Contracts.PasswordService.Requests
{
    public sealed record UpdateFolderRequest(string Id, string? ParentFolderId, string Name);
}