using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RestoreAllFromTrash
{
    public sealed record RestoreAllVaultsFromTrashCommand(Guid UserId) : IRequest<Result<Unit>>;
}