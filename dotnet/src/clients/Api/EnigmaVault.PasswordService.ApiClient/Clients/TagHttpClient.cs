using Common.Core.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.PasswordService.ApiClient.Clients
{
    public sealed class TagService(HttpClient httpClient) : ITagService
    {
        private readonly HttpClient _httpClient = httpClient;
        private string _url = "api/tags";
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<List<TagResponse>>> GetAll()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_url}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<TagResponse>>() ?? [];
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, ex.Message);
            }
        }

        public async Task<Result<string>> CreateAsync(CreateTagRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_url, request, _jsonSerializerOptions); 
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<string>(_jsonSerializerOptions) ?? "";
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, ex.Message);
            }
        }

        public async Task<Result> DeleteAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_url}/{id}");
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(Error.New(ErrorCode.ApiError, ex.Message));
            }
        }

        public async Task<Result> UpdateAsync(UpdateTagRequest request)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync(_url, request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(Error.New(ErrorCode.ApiError, ex.Message));
            }
        }
    }
}