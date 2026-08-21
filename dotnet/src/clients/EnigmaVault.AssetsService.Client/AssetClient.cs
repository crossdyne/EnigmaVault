using System.Text.Json;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Options;
using Shared.Contracts.AssetsService.Clients;
using Shared.Contracts.AssetsService.Responses;
using Shared.Contracts.FileService.Constants;
using Shared.Http;

namespace EnigmaVault.AssetsService.Client
{
    public sealed class AssetClient(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value),  IAssetClient
    {
        public async Task<Result<List<IconMetadataResponse>>> GetFilesMetadata(CancellationToken cancellationToken = default)
            => await CatchResponseAsync<List<IconMetadataResponse>>(async ct => await _http.GetAsync($"api/v1/aggregated/asset/{ProjectConstants.EnigmaVault}", ct), cancellationToken);
    }
}