using Common.Core.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.ApiClient.Clients
{
    public interface IIconCategoryService
    {
        Task<Result<string>> CreatePersonalAsync(CreateIconCategoryPersonalRequest request);
        Task<Result<Unit>> UpdatePersonalAsync(UpdatePersonalIconCategoryRequest request);
        Task<Result<Unit>> DeletePersonalAsync(string id);
        Task<Result<List<IconCategoryResponse>>> GetAllAsync();
    }
}