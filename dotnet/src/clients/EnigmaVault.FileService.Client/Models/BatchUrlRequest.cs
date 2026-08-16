namespace EnigmaVault.FileService.Client.Models
{
    public sealed record BatchUrlRequest(List<FileRequest> Files, int? Expires);
}