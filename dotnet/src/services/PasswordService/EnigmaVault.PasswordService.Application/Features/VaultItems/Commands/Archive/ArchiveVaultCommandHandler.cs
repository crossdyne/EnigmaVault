using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Archive
{
    public sealed class ArchiveVaultCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ArchiveVaultCommand, Result<Unit>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(ArchiveVaultCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mayBe = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

                if (mayBe.IsNone)
                    return new Error(ErrorCode.NotFound, $"Запись {request.VaultItemId} не была найдена");

                mayBe.Value.SetArchive(true);

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