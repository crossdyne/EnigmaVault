using Crossdyne.Toolkit.Results;
using EnigmaVault.FileService.Client.Models;

namespace EnigmaVault.FileService.Client.Clients
{
    public interface IFileServiceClient
    {
        Task<Result<BatchUrlResponse>> GetUrls(BatchUrlRequest request, CancellationToken cancellationToken = default);
    }
}