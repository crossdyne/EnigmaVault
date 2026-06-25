namespace EnigmaVault.FileService.ApiClient.Models
{
    public sealed record BatchUrlRequest(List<FileRequest> Files, int? Expires);
}