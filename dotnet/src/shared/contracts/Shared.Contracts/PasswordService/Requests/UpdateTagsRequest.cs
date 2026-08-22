namespace Shared.Contracts.PasswordService.Requests
{
    public sealed record UpdateTagsRequest(List<string> TagIds);
}