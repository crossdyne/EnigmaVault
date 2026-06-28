using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.Enums;
using EnigmaVault.PasswordService.Domain.Models;
using EnigmaVault.PasswordService.Domain.ValueObjects.Password;
using EnigmaVault.PasswordService.Domain.ValueObjects.User;
using MediatR;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Create
{
    public sealed class CreateVaultItemCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateVaultItemCommand, Result<string>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<string>> Handle(CreateVaultItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var type = Enum.Parse<VaultType>(request.PasswordType);
                var vaultItem = VaultItem.Create(UserId.Create(request.UserId), type, IconId.Create(request.IconId), EncryptedData.Create(request.EncryptedOverview), EncryptedData.Create(request.EncryptedDetails));

                await _vaultItemRepository.AddAsync(vaultItem, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return vaultItem.Id.ToString();
            }
            catch (DomainException ex)
            {
                return ex.Error;
            }
            catch (Exception ex)
            {
                return new Error(ErrorCode.Server, $"Произошла непредвиденная ошибка: {ex}");
            }
        }
    }
}