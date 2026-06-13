using Common.Core.Results;
using Shared.Contracts.Requests;
using Shared.Contracts.Requests.Authentication;
using Shared.Contracts.Responses.Authentication;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.Authentication.ApiClient.HttpClients
{
    public sealed class AuthService(HttpClient httpClient) : IAuthService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<SrpChallengeResponse>> GetSrpChallenge(SrpChallengeRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/srp/challenge", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var resultData = await response.Content.ReadFromJsonAsync<SrpChallengeResponse>();

                return resultData!;
            }
            catch (HttpRequestException ex)
            {
                return Error.New(ErrorCode.ApiError, ex.Message);
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, $"Произошла критическая ошибки при отправки запроса: {ex.Message}");
            }
        }

        public async Task<Result<AuthResponse>> VerifySrpProof(SrpVerifyRequest request)
        {
            HttpResponseMessage? response = null!;
            try
            {

                response = await _httpClient.PostAsJsonAsync("api/auth/srp/verify", request, _jsonSerializerOptions); ;

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Error.New(ErrorCode.ApiError,
                        $"HTTP {response.StatusCode}: {errorContent}");
                }

                var resultData = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (resultData is null)
                {
                    return Error.New(ErrorCode.ApiError, "Пустой или некорректный JSON-ответ от сервера");
                }

                return resultData!;
            }
            catch (JsonException ex) when (ex.Message.Contains("could not be converted"))
            {
                var rawContent = await response.Content.ReadAsStringAsync();
                return Error.New(ErrorCode.ApiError, $"Ошибка десериализации AuthResponse. Ответ сервера: {rawContent}\nОшибка: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                return Error.New(ErrorCode.ApiError, ex.Message);
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, $"Произошла критическая ошибка при отправке запроса: {ex.Message}");
            }

        }

        public async Task<Result<AuthResponse?>> LoginByToken(LoginByTokenRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadFromJsonAsync<AuthResponse>();

                return responseData;
            }
            catch (HttpRequestException ex)
            {
                return Error.New(ErrorCode.ApiError, ex.Message);
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, $"Произошла критическая ошибки при отправки запроса: {ex.Message}");
            }
        }

        public async Task<Result<AuthResponse?>> Refresh(RefreshTokenRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/token-login", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadFromJsonAsync<AuthResponse>();

                return responseData;
            }
            catch (HttpRequestException ex)
            {
                return Error.New(ErrorCode.ApiError, ex.Message);
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, $"Произошла критическая ошибки при отправки запроса: {ex.Message}");
            }
        }
    }
}