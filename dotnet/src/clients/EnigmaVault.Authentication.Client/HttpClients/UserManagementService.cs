using Crossdyne.Toolkit.Results;
using EnigmaVault.Authentication.Client.Model.Responses;
using Microsoft.Extensions.Options;
using Shared.Contracts.Responses;
using Shared.Contracts.Responses.UserManagement;
using Shared.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EnigmaVault.Authentication.Client.HttpClients
{
    public sealed class UserManagementService(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value), IUserManagementService
    {
        public async Task<Result<UserResponse>> Me(string accessToken, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<UserResponse>(async ct =>
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return await _http.GetAsync("api/v1/users/me", ct);
            }, cancellationToken);

        public async Task<Result<UserPublicInfo>> GetPublicEncryptionInfo(string login, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<UserPublicInfo>(async ct => await _http.GetAsync($"api/v1/users/{login}/crypto/public", ct), cancellationToken);

        public async Task<Result<DekResponse>> GetDek(CancellationToken cancellationToken = default)
            => await CatchResponseAsync<DekResponse>(async ct => await _http.GetAsync($"api/v1/users/private/crypto/dek", ct), cancellationToken);
    }
}