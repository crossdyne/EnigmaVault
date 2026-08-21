using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;

namespace Shared.Contracts.Clients.PasswordsService
{
    public interface ITagService
    {
        Task<Result<List<TagResponse>>> GetAll(CancellationToken cancellationToken = default);
        Task<Result<string>> CreateAsync(CreateTagRequest request, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<Unit>> UpdateAsync(UpdateTagRequest request, CancellationToken cancellationToken = default);
    }
}