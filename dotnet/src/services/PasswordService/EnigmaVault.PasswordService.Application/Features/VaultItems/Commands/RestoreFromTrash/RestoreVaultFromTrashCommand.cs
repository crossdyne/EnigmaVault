using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RestoreFromTrash
{
    public sealed record RestoreVaultFromTrashCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<Unit>>;
}