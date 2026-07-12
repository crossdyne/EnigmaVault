using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;
using Shared.Kernel.Errors;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.PasswordService.Client.Clients
{
    public sealed class VaultService(HttpClient client) : IVaultService
    {
        private readonly HttpClient _httpClient = client;
        private readonly string _url = "api/vault";
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<string>> CreateAsync(CreateVaultItemRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_url, request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync() ?? "";
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.ToString());
            }
        }

        public async Task<Result<string>> UpdateAsync(UpdateVaultItemRequest request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(_url, request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                return Result<string>.Success(result);
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> AddToFavoritesAsync(string vaultId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/add-favorites/{vaultId}", null);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> RemoveFromFavoritesAsync(string vaultId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/remove-favorites/{vaultId}", null);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> ArchiveAsync(string vaultId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/archive/{vaultId}", null);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> UnArchiveAsync(string vaultId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/un-archive/{vaultId}", null);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> RestoreAllFromArchiveAsync()
        {   
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/un-archive/all", null);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> DeleteAsync(string vaultId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_url}/{vaultId}");
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<DateTime>> MoveToTrashAsync(string vaultId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/move-to-trash/{vaultId}", null);
                response.EnsureSuccessStatusCode();

                var dateTimeValue = await response.Content.ReadFromJsonAsync<DateTime>();
                var dateTimeLocal = dateTimeValue.ToLocalTime();

                return Result<DateTime>.Success(dateTimeLocal);
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> RestoreFromTrashAsync(string vaultId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/restore-from-trash/{vaultId}", null);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> RestoreAllFromTrashAsync()
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/restore-all-from-trash", null);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> EmptyTrashAsync()
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/empty-trash", null);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<List<EncryptedVaultResponse>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_url}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<EncryptedVaultResponse>>(_jsonSerializerOptions) ?? [];
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<EncryptedVaultResponse>> GetById(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_url}/{id}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<EncryptedVaultResponse>(_jsonSerializerOptions);
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> AddTagAsync(string vaultId, string tagId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/add-tag/{vaultId}/{tagId}", null);
                response.EnsureSuccessStatusCode();

                return Unit.Value;
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> RemoveTagAsync(string vaultId, string tagId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/remove-tag/{vaultId}/{tagId}", null);
                response.EnsureSuccessStatusCode();

                return Unit.Value;
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }

        public async Task<Result<Unit>> ChangeIcon(string vaultId, string iconId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{_url}/change/{vaultId}/icon/{iconId}", null);
                response.EnsureSuccessStatusCode();

                return Unit.Value;
            }
            catch (Exception ex)
            {
                return new Error(AppErrors.ApiError, ex.Message);
            }
        }
    }
}