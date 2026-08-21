namespace Shared.Contracts.PasswordService.Requests
{
    public sealed record UpdateTagRequest(string Id, string Name, string Color);
}