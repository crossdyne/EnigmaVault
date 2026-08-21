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
    public sealed class TagService(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value), ITagService
    {
        private string _url = "api/tags";

        public async Task<Result<List<TagResponse>>> GetAll(CancellationToken cancellationToken = default)
            => await CatchResponseAsync<List<TagResponse>>(async ct => await _http.GetAsync($"{_url}", ct), cancellationToken);

        public async Task<Result<string>> CreateAsync(CreateTagRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<string>(async ct => await _http.PostAsJsonAsync(_url, request, _jsonOptions, ct), cancellationToken);

        public async Task<Result<Unit>> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.DeleteAsync($"{_url}/{id}", ct), cancellationToken);

        public async Task<Result<Unit>> UpdateAsync(UpdateTagRequest request, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsJsonAsync(_url, request, _jsonOptions), cancellationToken);
    }
}