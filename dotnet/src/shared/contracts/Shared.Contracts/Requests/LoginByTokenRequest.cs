namespace Shared.Contracts.Requests
{
    public record LoginByTokenRequest(string RefreshToken, string AccessToken);
}