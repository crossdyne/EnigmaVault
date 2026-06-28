namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdateTagRequest(string Id, string Name, string Color);
}