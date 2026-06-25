namespace EnigmaVault.AssetsService.ApiClient.Models
{
    public sealed record S3KeyResponse(string Key, string Bucket, string Name, string FolderPath);
}