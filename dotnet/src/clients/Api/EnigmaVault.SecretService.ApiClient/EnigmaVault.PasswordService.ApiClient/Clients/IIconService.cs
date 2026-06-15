using Common.Core.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.ApiClient.Clients
{
    public interface IIconService
    {
        Task<Result<string>> CreatePersonalAsync(CreateIconPersonalRequest request);
        Task<Result<Unit>> DeletePersonalAsync(string userId, string id);
        Task<Result<List<IconResponse>>> GetAll();
    }
}