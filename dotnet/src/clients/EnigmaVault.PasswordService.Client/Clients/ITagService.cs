using Crossdyne.Toolkit.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Client.Clients
{
    public interface ITagService
    {
        Task<Result<List<TagResponse>>> GetAll();
        Task<Result<string>> CreateAsync(CreateTagRequest request);
        Task<Result> DeleteAsync(string id);
        Task<Result> UpdateAsync(UpdateTagRequest request);
    }
}