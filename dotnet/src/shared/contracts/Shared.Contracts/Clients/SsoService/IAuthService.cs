using Crossdyne.Toolkit.Results;
using Shared.Contracts.Requests;
using Shared.Contracts.Requests.Authentication;
using Shared.Contracts.Responses.Authentication;

namespace Shared.Contracts.Clients.SsoService
{
    public interface IAuthService
    {
        Task<Result<SrpChallengeResponse>> GetSrpChallenge(SrpChallengeRequest request, CancellationToken cancellationToken = default);
        Task<Result<AuthResponse>> VerifySrpProof(SrpVerifyRequest request, CancellationToken cancellationToken = default);
        Task<Result<AuthResponse>> RefreshTokens(LoginByTokenRequest request, CancellationToken cancellationToken = default);
    }
}