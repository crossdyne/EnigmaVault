using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.ValueObjects.Password;
using MediatR;
using EncryptedData = EnigmaVault.PasswordService.Domain.ValueObjects.Password.EncryptedData;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Update
{
    public sealed class UpdateVaultItemCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateVaultItemCommand, Result<string>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<string>> Handle(UpdateVaultItemCommand request, CancellationToken cancellationToken)
        {
            var maybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);
            
            if (maybeVault.IsNone)
                return Result<string>.Failure(new Error(ErrorCode.NotFound, $"Данный элемент {request.VaultItemId} не был найден."));

            var vault = maybeVault.Value;

            vault.UpdateOverview(EncryptedData.Create(request.EncryptedOverview));
            vault.UpdateDetails(EncryptedData.Create(request.EncryptedDetails));
            vault.SetIcon(IconId.Create(request.IconId));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(vault.DateUpdated!.Value.ToString("o")!);
        }
    }
}