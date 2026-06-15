namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdatePersonalIconRequest(string Id, string Name, string SvgCode, string IconCategoryId);
}