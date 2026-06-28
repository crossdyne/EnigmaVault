namespace Shared.Contracts.Responses.PasswordService
{
    public sealed record IconResponse(string Id, string? UserId, string SvgCode, string IconName, string IconCategoryId);
}