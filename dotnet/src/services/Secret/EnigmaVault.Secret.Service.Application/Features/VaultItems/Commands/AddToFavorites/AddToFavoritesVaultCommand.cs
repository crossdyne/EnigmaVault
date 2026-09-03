using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Service.Application.Features.VaultItems.Commands.AddToFavorites
{
    public sealed record AddToFavoritesVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result<Unit>>;
}