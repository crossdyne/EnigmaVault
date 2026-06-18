using Common.Core.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.PasswordService.ApiClient.Clients
{
    public sealed class IconCategoryService(HttpClient client) : IIconCategoryService
    {
		private readonly HttpClient _httpClient = client;
        private const string _url = "api/icon-categories";
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<string>> CreatePersonalAsync(CreateIconCategoryPersonalRequest request)
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.PostAsJsonAsync($"{_url}/personal", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
			catch (Exception ex)
			{
                return Error.New(ErrorCode.ApiError, ex.ToString());
            }
        }

        public async Task<Result<Unit>> UpdatePersonalAsync(UpdatePersonalIconCategoryRequest request)
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.PatchAsJsonAsync($"{_url}/personal", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                return Unit.Value;
            }
			catch (Exception ex)
			{
                return Error.New(ErrorCode.ApiError, ex.ToString());
            }
        }

        public async Task<Result<Unit>> DeletePersonalAsync(string id)
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.DeleteAsync($"{_url}/personal/{id}");
                response.EnsureSuccessStatusCode();

                return Unit.Value;
            }
			catch (Exception ex)
			{
                return Error.New(ErrorCode.ApiError, ex.ToString());
            }
        }

        public async Task<Result<List<IconCategoryResponse>>> GetAllAsync()
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.GetAsync($"{_url}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<IconCategoryResponse>>(_jsonSerializerOptions) ?? [];
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, ex.ToString());
            }
        }
    }
}