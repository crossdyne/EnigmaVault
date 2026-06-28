using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.AddTag
{
    public sealed record AddTagToVaulItemCommand(Guid UserId, Guid VaultItemId, Guid TagId) : IRequest<Result<Unit>>;
}