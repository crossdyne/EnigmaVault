namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateSubFolderRequest(string ParentFolderId, string Name, string Color);
}