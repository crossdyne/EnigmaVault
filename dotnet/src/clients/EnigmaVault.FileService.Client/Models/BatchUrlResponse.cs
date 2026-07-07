namespace EnigmaVault.FileService.Client.Models
{
    public sealed record BatchUrlResponse(string Status, int ExpiresIn, List<FileUrl> Urls, List<FileError> Errors, string Reason);
}