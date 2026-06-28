using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.EmptyTrash
{
    public sealed record EmptyVaultsTrashCommand(Guid UserId) : IRequest<Result<Unit>>;
}