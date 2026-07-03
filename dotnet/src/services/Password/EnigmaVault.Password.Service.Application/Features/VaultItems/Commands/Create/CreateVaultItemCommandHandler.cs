using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Application.Common.Repositories;
using EnigmaVault.Password.Service.Domain.Enums;
using EnigmaVault.Password.Service.Domain.Models;
using EnigmaVault.Password.Service.Domain.ValueObjects.Password;
using EnigmaVault.Password.Service.Domain.ValueObjects.User;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.Create
{
    public sealed class CreateVaultItemCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateVaultItemCommand, Result<string>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<string>> Handle(CreateVaultItemCommand request, CancellationToken cancellationToken)
        {
            var type = Enum.Parse<VaultType>(request.PasswordType);
            var vaultItem = VaultItem.Create(UserId.Create(request.UserId), type, IconId.Create(request.IconId), EncryptedData.Create(request.EncryptedOverview), EncryptedData.Create(request.EncryptedDetails));

            await _vaultItemRepository.AddAsync(vaultItem, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return vaultItem.Id.ToString();
        }
    }
}