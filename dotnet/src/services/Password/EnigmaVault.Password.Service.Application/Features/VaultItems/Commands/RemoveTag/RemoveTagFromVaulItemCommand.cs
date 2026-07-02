using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.RemoveTag
{
    public sealed record RemoveTagFromVaultItemCommand(Guid UserId, Guid VaultItemId, Guid TagId) : IRequest<Result<Unit>>;
}