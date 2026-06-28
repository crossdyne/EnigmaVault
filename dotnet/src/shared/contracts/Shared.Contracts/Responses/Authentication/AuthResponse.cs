namespace Shared.Contracts.Responses.Authentication
{
    public sealed record AuthResponse(string AccessToken, string RefreshToken, string? M2 = null);
}