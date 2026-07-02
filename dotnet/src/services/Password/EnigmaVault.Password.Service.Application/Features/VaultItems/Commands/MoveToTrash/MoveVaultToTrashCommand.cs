using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.MoveToTrash
{
    public sealed record MoveVaultToTrashCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<DateTime>>;
}