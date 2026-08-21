using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Options;
using Shared.Contracts.PasswordService.Clients;
using Shared.Contracts.PasswordService.Requests;
using Shared.Contracts.PasswordService.Responses;
using Shared.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.PasswordService.Client.Clients
{
    public sealed class VaultService(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value), IVaultService
    {
        private readonly string _url = "api/vault";

        public async Task<Result<string>> CreateAsync(CreateVaultItemRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<string>(async ct => await _http.PostAsJsonAsync(_url, request, _jsonOptions, ct), cancellationToken);

        public async Task<Result<string>> UpdateAsync(UpdateVaultItemRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<string>(async ct => await _http.PutAsJsonAsync(_url, request, _jsonOptions, ct), cancellationToken);

        public async Task<Result<Unit>> AddToFavoritesAsync(string vaultId, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/add-favorites/{vaultId}", null, ct), cancellationToken);

        public async Task<Result<Unit>> RemoveFromFavoritesAsync(string vaultId, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/remove-favorites/{vaultId}", null, ct), cancellationToken);

        public async Task<Result<Unit>> ArchiveAsync(string vaultId, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/archive/{vaultId}", null, ct), cancellationToken);

        public async Task<Result<Unit>> UnArchiveAsync(string vaultId, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/un-archive/{vaultId}", null, ct), cancellationToken);

        public async Task<Result<Unit>> RestoreAllFromArchiveAsync(CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/un-archive/all", null, ct), cancellationToken);

        public async Task<Result<Unit>> DeleteAsync(string vaultId, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.DeleteAsync($"{_url}/{vaultId}", ct), cancellationToken);

        public async Task<Result<DateTime>> MoveToTrashAsync(string vaultId, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<DateTime>(async ct => await _http.PatchAsync($"{_url}/move-to-trash/{vaultId}", null, ct), cancellationToken);

        public async Task<Result<Unit>> RestoreFromTrashAsync(string vaultId, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/restore-from-trash/{vaultId}", null, ct), cancellationToken);

        public async Task<Result<Unit>> RestoreAllFromTrashAsync(CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/restore-all-from-trash", null, ct), cancellationToken);

        public async Task<Result<Unit>> EmptyTrashAsync(CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/empty-trash", null, ct), cancellationToken);

        public async Task<Result<List<EncryptedVaultResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
            => await CatchResponseAsync<List<EncryptedVaultResponse>>(async ct => await _http.GetAsync($"{_url}", ct), cancellationToken);

        public async Task<Result<EncryptedVaultResponse>> GetById(string id, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<EncryptedVaultResponse>(async ct => await _http.GetAsync($"{_url}/{id}", ct), cancellationToken);

        public async Task<Result<Unit>> AddTagAsync(string vaultId, string tagId, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/add-tag/{vaultId}/{tagId}", null, ct), cancellationToken);

        public async Task<Result<Unit>> RemoveTagAsync(string vaultId, string tagId, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsync($"{_url}/remove-tag/{vaultId}/{tagId}", null, ct), cancellationToken);

        public async Task<Result<DateUpdateResponse>> ChangeIcon(string vaultId, string iconId, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<DateUpdateResponse>(async ct => await _http.PatchAsync($"{_url}/change/{vaultId}/icon/{iconId}", null, ct), cancellationToken);

        public async Task<Result<DateUpdateResponse>> UpdateTagsAsync(string vaultId, UpdateTagsRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<DateUpdateResponse>(async ct => await _http.PatchAsJsonAsync($"{_url}/{vaultId}/tags", request, ct), cancellationToken);

        public async Task<Result<PasswordsCountRecordsResponse>> CountRecordsAsync(CancellationToken cancellationToken = default)
            => await CatchResponseAsync<PasswordsCountRecordsResponse>(async ct => await _http.GetAsync($"{_url}/records/count", ct), cancellationToken);
    }
}