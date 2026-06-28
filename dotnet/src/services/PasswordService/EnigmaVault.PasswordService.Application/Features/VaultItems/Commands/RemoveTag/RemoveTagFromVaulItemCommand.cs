using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RemoveTag
{
    public sealed record RemoveTagFromVaulItemCommand(Guid UserId, Guid VaultItemId, Guid TagId) : IRequest<Result<Unit>>;
}