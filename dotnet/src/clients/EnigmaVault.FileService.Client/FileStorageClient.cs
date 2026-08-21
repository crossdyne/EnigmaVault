using System.Net.Http.Json;
using System.Text.Json;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Options;
using Shared.Contracts.Clients.FileService;
using Shared.Contracts.Requests.FileService;
using Shared.Contracts.Responses.FileService;
using Shared.Http;

namespace EnigmaVault.FileService.Client
{
    public sealed class FileStorageClient(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value), IFileServiceClient
    {
        public async Task<Result<BatchUrlResponse>> GetUrls(BatchUrlRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<BatchUrlResponse>(async ct => await _http.PostAsJsonAsync("api/files/urls", request, ct), cancellationToken);
    }
}