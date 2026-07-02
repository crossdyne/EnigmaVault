using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Queries.GetDetails
{
    public sealed record GetEncryptedDetailsQuery(Guid UserId, Guid VaultItemId) : IRequest<Result<string>>;
}