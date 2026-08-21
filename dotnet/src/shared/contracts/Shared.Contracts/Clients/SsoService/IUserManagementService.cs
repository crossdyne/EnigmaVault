using Crossdyne.Toolkit.Results;
using Shared.Contracts.Responses;
using Shared.Contracts.Responses.Authentication;
using Shared.Contracts.Responses.UserManagement;

namespace Shared.Contracts.Clients.SsoService
{
    public interface IUserManagementService
    {
        Task<Result<UserResponse>> Me(string accessToken, CancellationToken cancellationToken = default);
        Task<Result<UserPublicInfo>> GetPublicEncryptionInfo(string confirmationToken, CancellationToken cancellationToken = default);
        Task<Result<DekResponse>> GetDek(CancellationToken cancellationToken = default);
    }
}