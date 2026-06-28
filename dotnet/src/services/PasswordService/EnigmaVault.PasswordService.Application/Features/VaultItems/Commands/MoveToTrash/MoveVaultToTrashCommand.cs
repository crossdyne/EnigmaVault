using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.MoveToTrash
{
    public sealed record MoveVaultToTrashCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<DateTime>>;
}