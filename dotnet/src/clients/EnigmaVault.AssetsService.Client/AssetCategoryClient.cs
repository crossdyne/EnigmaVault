using System.Text.Json;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Options;
using Shared.Contracts.Clients.AssetsService;
using Shared.Contracts.Responses.Assets;
using Shared.Http;

namespace EnigmaVault.AssetsService.Client
{
    public sealed class AssetCategoryClient(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value), IAssetCategoryClient
    {        
        public async Task<Result<List<IconCategoryResponse>>> GetIconCategories(CancellationToken cancellationToken = default)
            => await CatchResponseAsync<List<IconCategoryResponse>>(async ct => await _http.GetAsync($"api/v1/aggregated/category", ct), cancellationToken);
    }
}