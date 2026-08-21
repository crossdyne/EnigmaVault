namespace Shared.Contracts.PasswordService.Responses
{
    public sealed record IconResponse(string Id, string? UserId, string SvgCode, string IconName, string IconCategoryId);
}