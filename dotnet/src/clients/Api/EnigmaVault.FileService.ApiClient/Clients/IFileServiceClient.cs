using Common.Core.Results;
using EnigmaVault.FileService.ApiClient.Models;

namespace EnigmaVault.FileService.ApiClient.Clients
{
    public interface IFileServiceClient
    {
        Task<Result<BatchUrlResponse>> GetUrls(BatchUrlRequest request);
    }
}