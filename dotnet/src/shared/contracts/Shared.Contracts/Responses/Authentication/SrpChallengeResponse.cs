namespace Shared.Contracts.Responses.Authentication
{
    public sealed record SrpChallengeResponse(string Salt, string B);
}