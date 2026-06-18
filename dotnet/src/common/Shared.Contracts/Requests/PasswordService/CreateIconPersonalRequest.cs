namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateIconPersonalRequest(string SvgCode, string Name, string IconCategoryId);
}