namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdateFolderRequest(string Id, string? ParentFolderId, string Name);
}