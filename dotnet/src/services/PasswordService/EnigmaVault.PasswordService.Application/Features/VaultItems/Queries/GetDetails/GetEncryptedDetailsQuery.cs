using Common.Core.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Queries.GetDetails
{
    public sealed record GetEncryptedDetailsQuery(Guid UserId, Guid VaultItemId) : IRequest<Result<string>>;
}