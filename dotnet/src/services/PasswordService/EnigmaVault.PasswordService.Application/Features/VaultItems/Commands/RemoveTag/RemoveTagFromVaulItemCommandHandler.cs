using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.ValueObjects.Tag;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RemoveTag
{
    public sealed class RemoveTagFromVaulItemCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RemoveTagFromVaulItemCommand, Result<Unit>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(RemoveTagFromVaulItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

                if (maybeVault.IsNone)
                    return new Error(ErrorCode.NotFound,  $"Элемент {request.VaultItemId} не был найден.");

                var vault = maybeVault.Value;

                vault.RemoveTag(TagId.Create(request.TagId));

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception)
            {
                return new Error(ErrorCode.Server, "Произошла непредвиденная ошибка.");
            }
        }
    }
}