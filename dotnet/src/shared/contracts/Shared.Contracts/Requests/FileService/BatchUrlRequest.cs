using Shared.Contracts.Responses.FileService;

namespace Shared.Contracts.Requests.FileService
{
    public sealed record BatchUrlRequest(List<FileRequest> Files, int? Expires);
}