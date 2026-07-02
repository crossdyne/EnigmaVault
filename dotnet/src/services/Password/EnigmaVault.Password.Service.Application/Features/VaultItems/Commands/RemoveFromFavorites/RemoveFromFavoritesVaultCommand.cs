using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.RemoveFromFavorites
{
    public sealed record RemoveFromFavoritesVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result<Unit>>;
}