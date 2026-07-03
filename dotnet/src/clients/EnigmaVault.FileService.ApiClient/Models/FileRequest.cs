namespace EnigmaVault.FileService.ApiClient.Models
{
    public sealed record FileRequest(string Bucket, string Folder, string Key);
}