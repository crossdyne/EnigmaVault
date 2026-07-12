namespace EnigmaVault.FileService.Client.Models
{
    public sealed record FileRequest(string Bucket, string Folder, string Key);
}