namespace Shared.Contracts.PasswordService.Requests
{
    public sealed record CreateSubFolderRequest(string ParentFolderId, string Name, string Color);
}