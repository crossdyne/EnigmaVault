using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.UnArchive
{
    public sealed class UnArchiveVaultCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<UnArchiveVaultCommand, Result>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(UnArchiveVaultCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mayBe = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

                if (mayBe.IsNone)
                    return Result.Failure(new Error(ErrorCode.NotFound, $"Данный элемент {request.VaultItemId} не был найден"));

                mayBe.Value.SetArchive(false);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Server, "Произошла непредвиденная ошибка."));
            }
        }
    }
}