namespace Shared.Contracts.Responses.Assets
{
    public sealed record S3KeyResponse(string Key, string Bucket, string Name, string FolderPath);
}