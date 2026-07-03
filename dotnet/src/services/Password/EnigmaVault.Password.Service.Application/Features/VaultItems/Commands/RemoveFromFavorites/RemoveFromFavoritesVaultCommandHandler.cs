using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Application.Common.Repositories;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.RemoveFromFavorites
{
    public sealed class RemoveFromFavoritesVaultCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RemoveFromFavoritesVaultCommand, Result<Unit>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(RemoveFromFavoritesVaultCommand request, CancellationToken cancellationToken)
        {
            var maybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

            if (maybeVault.IsNone)
                return new Error(ErrorCode.NotFound, $"Элемент {request.VaultItemId} не был найден");

            var vault = maybeVault.Value;

            vault.SetFavorite(false);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}