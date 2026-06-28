using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Delete
{
    public sealed class DeleteVaultItemCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteVaultItemCommand, Result<Unit>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(DeleteVaultItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vaybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

                if (vaybeVault.HasValue)
                {
                    _vaultItemRepository.Remove(vaybeVault.Value);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }

                return new Error(ErrorCode.NotFound, $"Запись {request.VaultItemId} не была найдена.");
            }
            catch (Exception)
            {
                return new Error(ErrorCode.Server, "Произошла непредвиденная ошибка на стороне сервера.");
            }
        }
    }
}