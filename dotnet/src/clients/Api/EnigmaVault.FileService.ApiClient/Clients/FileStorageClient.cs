using System.Net.Http.Json;
using Crossdyne.Toolkit.Results;
using EnigmaVault.FileService.ApiClient.Models;
using Shared.Kernel.Errors;

namespace EnigmaVault.FileService.ApiClient.Clients
{
    public sealed class FileStorageClient(HttpClient http) : IFileServiceClient
    {
        public async Task<Result<BatchUrlResponse>> GetUrls(BatchUrlRequest request)
        {
            var response = await http.PostAsJsonAsync("api/files/urls", request);
            
            if (!response.IsSuccessStatusCode)
                return Result<BatchUrlResponse>.Failure(new Error(AppErrors.ApiError, await response.Content.ReadAsStringAsync()));

            var result = await response.Content.ReadFromJsonAsync<BatchUrlResponse>();

            return Result<BatchUrlResponse>.Success(result!);
        }
    }
}