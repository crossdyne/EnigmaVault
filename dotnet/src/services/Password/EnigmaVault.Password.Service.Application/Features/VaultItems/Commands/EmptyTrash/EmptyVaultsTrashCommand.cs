using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.EmptyTrash
{
    public sealed record EmptyVaultsTrashCommand(Guid UserId) : IRequest<Result<Unit>>;
}