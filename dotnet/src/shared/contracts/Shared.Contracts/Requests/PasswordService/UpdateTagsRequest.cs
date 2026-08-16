namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdateTagsRequest(List<string> TagIds);
}