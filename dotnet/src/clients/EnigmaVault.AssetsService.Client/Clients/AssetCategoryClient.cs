using System.Net.Http.Json;
using System.Text.Json;
using Crossdyne.Toolkit.Results;
using EnigmaVault.AssetsService.Client.Models;
using Shared.Kernel.Errors;

namespace EnigmaVault.AssetsService.Client.Clients
{
    public sealed class AssetCategoryClient(HttpClient http) : IAssetCategoryClient
    {        
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<List<IconCategoryResponse>>> GetIconCategories()
        {
            try
            {
                var response = await http.GetAsync($"api/v1/aggregated/category");

                response.EnsureSuccessStatusCode();

                var icons = await response.Content.ReadFromJsonAsync<List<IconCategoryResponse>>(_jsonSerializerOptions) ?? [];

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