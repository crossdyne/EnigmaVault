using Common.Core.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.PasswordService.ApiClient.Clients
{
    public sealed class IconService(HttpClient client) : IIconService 
    {
        private readonly HttpClient _httpClient = client;
        private const string _url = "api/icons";
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<string>> CreatePersonalAsync(CreateIconPersonalRequest request)
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

        public async Task<Result<Unit>> DeletePersonalAsync(string userId, string id)
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.DeleteAsync($"personal/{userId}/{id}");
                response.EnsureSuccessStatusCode();

                return Unit.Value;
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, ex.ToString());
            }
        }

        public async Task<Result<List<IconResponse>>> GetAll()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_url}/all"); 
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<IconResponse>>(_jsonSerializerOptions) ?? [];
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, ex.ToString());
            }
        }
    }
}