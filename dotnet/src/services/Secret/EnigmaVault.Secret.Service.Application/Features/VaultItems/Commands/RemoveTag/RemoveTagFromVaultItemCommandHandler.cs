using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Service.Application.Common;
using EnigmaVault.Secret.Service.Application.Common.Repositories;
using EnigmaVault.Secret.Service.Domain.ValueObjects.Tag;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Service.Application.Features.VaultItems.Commands.RemoveTag
{
    public sealed class RemoveTagFromVaultItemCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RemoveTagFromVaultItemCommand, Result<Unit>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(RemoveTagFromVaultItemCommand request, CancellationToken cancellationToken)
        {
            var maybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

            if (maybeVault.IsNone)
                return new Error(ErrorCode.NotFound,  $"Элемент {request.VaultItemId} не был найден.");

            var vault = maybeVault.Value;

            vault.RemoveTag(TagId.Create(request.TagId));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}