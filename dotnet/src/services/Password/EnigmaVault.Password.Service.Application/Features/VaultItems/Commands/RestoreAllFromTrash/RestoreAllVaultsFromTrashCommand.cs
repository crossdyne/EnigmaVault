using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.RestoreAllFromTrash
{
    public sealed record RestoreAllVaultsFromTrashCommand(Guid UserId) : IRequest<Result<Unit>>;
}