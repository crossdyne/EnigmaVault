namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdateCommonIconRequest(string Id, string Name, string SvgCode, string IconCategoryId);
}