using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.VaultItems.Commands.RestoreAllFromTrash
{
    public sealed record RestoreAllVaultsFromTrashCommand(Guid UserId) : IRequest<Result<Unit>>;
}