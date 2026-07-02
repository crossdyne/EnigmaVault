using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.Delete
{
    public sealed record DeleteVaultItemCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<Unit>>;
}