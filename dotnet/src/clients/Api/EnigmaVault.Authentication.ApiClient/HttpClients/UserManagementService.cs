using Common.Core.Results;
using EnigmaVault.Authentication.ApiClient.Model.Responses;
using Shared.Contracts.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.Authentication.ApiClient.HttpClients
{
    public sealed class UserManagementService(HttpClient httpClient) : IUserManagementService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<UserResponse?>> Me(string accesToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesToken);

                var responseMessage = await _httpClient.GetAsync("api/v1/users/me");

                responseMessage.EnsureSuccessStatusCode();
                var userResponse = await responseMessage.Content.ReadFromJsonAsync<UserResponse>(_jsonSerializerOptions);

                return Result<UserResponse?>.Success(userResponse);
            }
            catch (HttpRequestException ex)
            {
                return Result<UserResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка: {ex}"));
            }
            catch (JsonException jsonEx)
            {
                return Result<UserResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от api/users/login: {jsonEx.Message}"));
            }
        }

        public async Task<Result<UserPublicInfo>> GetPublicEncryptionInfo(string login)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync($"api/v1/users/{login}/crypto/public");
                responseMessage.EnsureSuccessStatusCode();

                var userPublicInfo = await responseMessage.Content.ReadFromJsonAsync<UserPublicInfo>(_jsonSerializerOptions);

                if (userPublicInfo == null)
                    return Result<UserPublicInfo>.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден."));

                return Result<UserPublicInfo>.Success(userPublicInfo);
            }
            catch (HttpRequestException ex)
            {
                return Result<UserPublicInfo>.Failure(new Error(ErrorCode.Create, $"Ошибка: {ex}"));
            }
            catch (JsonException jsonEx)
            {
                return Result<UserPublicInfo>.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от api/users/public-encryption-info: {jsonEx.Message}"));
            }
        }
    }
}