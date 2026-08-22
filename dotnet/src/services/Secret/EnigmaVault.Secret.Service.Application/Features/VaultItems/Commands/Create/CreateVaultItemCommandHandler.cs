using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Service.Application.Common;
using EnigmaVault.Secret.Service.Application.Common.Repositories;
using EnigmaVault.Secret.Service.Domain.Models;
using EnigmaVault.Secret.Service.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Service.Domain.ValueObjects.User;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.VaultItems.Commands.Create
{
    public sealed class CreateVaultItemCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateVaultItemCommand, Result<string>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<string>> Handle(CreateVaultItemCommand request, CancellationToken cancellationToken)
        {
            var vaultItem = VaultItem.Create(
                UserId.Create(request.UserId), 
                VaultType.Create(request.PasswordType), 
                IconId.Create(request.IconId), 
                EncryptedData.Create(request.EncryptedOverview), 
                EncryptedData.Create(request.EncryptedDetails), 
                CryptoVersion.Create(request.CryptoVersion));

            await _vaultItemRepository.AddAsync(vaultItem, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return vaultItem.Id.ToString();
        }
    }
}