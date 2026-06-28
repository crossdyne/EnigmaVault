using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Delete
{
    public sealed record DeleteVaultItemCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<Unit>>;
}