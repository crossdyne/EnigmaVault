using Crossdyne.Toolkit.Results;
using EnigmaVault.Authentication.Client.Model.Responses;
using Shared.Contracts.Responses;
using Shared.Contracts.Responses.UserManagement;

namespace EnigmaVault.Authentication.Client.HttpClients
{
    public interface IUserManagementService
    {
        Task<Result<UserResponse?>> Me(string accesToken);
        Task<Result<UserPublicInfo>> GetPublicEncryptionInfo(string confirmationToken);
        Task<Result<DekResponse>> GetDek();
    }
}