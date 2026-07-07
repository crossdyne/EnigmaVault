using System.Net.Http.Json;
using System.Text.Json;
using Crossdyne.Toolkit.Results;
using EnigmaVault.AssetsService.Client.Constants;
using EnigmaVault.AssetsService.Client.Models;
using Shared.Kernel.Errors;

namespace EnigmaVault.AssetsService.Client.Clients
{
    public sealed class AssetClient(HttpClient http) : IAssetClient
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<List<IconMetadataResponse>>> GetFilesMetadata()
        {
            try
            {
                var response = await http.GetAsync($"api/v1/aggregated/asset/{ProjectConstants.EnigmaVault}");

                response.EnsureSuccessStatusCode();

                var icons = await response.Content.ReadFromJsonAsync<List<IconMetadataResponse>>(_jsonSerializerOptions) ?? [];

                foreach (var item in icons)
                {
                    System.Console.WriteLine(item.AssetName);
                }

                return icons;
            }
            catch (HttpRequestException ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, $"Произошла критическая ошибки при отправки запроса: {ex.Message}");
            }
        }
    }
}