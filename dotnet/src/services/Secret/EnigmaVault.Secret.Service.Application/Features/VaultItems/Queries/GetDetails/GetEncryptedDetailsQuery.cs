using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.VaultItems.Queries.GetDetails
{
    public sealed record GetEncryptedDetailsQuery(Guid UserId, Guid VaultItemId) : IRequest<Result<string>>;
}