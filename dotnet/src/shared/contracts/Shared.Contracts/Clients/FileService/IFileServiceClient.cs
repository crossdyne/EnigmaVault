using Crossdyne.Toolkit.Results;
using Shared.Contracts.Requests.FileService;
using Shared.Contracts.Responses.FileService;

namespace Shared.Contracts.Clients.FileService
{
    public interface IFileServiceClient
    {
        Task<Result<BatchUrlResponse>> GetUrls(BatchUrlRequest request, CancellationToken cancellationToken = default);
    }
}