using Crossdyne.Toolkit.Results;
using EnigmaVault.Authentication.ApiClient.Model.Responses;
using Shared.Contracts.Responses;

namespace EnigmaVault.Authentication.ApiClient.HttpClients
{
    public interface IUserManagementService
    {
        Task<Result<UserResponse?>> Me(string accesToken);
        Task<Result<UserPublicInfo>> GetPublicEncryptionInfo(string confirmationToken);
    }
}