namespace Shared.Contracts.Responses.FileService
{
    public sealed record FileRequest(string Bucket, string Folder, string Key);
}