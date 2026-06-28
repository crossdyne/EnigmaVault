using Crossdyne.Toolkit.Results;
using Shared.Contracts.Requests;
using Shared.Contracts.Requests.Authentication;
using Shared.Contracts.Responses.Authentication;

namespace EnigmaVault.Authentication.ApiClient.HttpClients
{
    public interface IAuthService
    {
        Task<Result<SrpChallengeResponse>> GetSrpChallenge(SrpChallengeRequest request);
        Task<Result<AuthResponse>> VerifySrpProof(SrpVerifyRequest request);
        Task<Result<AuthResponse?>> RefreshTokens(LoginByTokenRequest request);
    }
}