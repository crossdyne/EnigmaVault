using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.AddToFavorites
{
    public sealed record AddToFavoritesVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result<Unit>>;
}