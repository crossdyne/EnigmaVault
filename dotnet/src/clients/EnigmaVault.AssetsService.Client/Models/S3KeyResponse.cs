namespace EnigmaVault.AssetsService.Client.Models
{
    public sealed record S3KeyResponse(string Key, string Bucket, string Name, string FolderPath);
}