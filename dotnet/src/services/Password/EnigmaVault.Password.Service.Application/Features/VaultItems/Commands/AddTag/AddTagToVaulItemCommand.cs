using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.AddTag
{
    public sealed record AddTagToVaultItemCommand(Guid UserId, Guid VaultItemId, Guid TagId) : IRequest<Result<Unit>>;
}