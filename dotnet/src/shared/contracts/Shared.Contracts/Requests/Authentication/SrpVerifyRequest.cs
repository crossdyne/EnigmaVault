namespace Shared.Contracts.Requests.Authentication
{
    public sealed record SrpVerifyRequest(string Login, string A, string M1);
}