using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.ValueObjects.Tag;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.AddTag
{
    public sealed class AddTagToVaulItemCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<AddTagToVaulItemCommand, Result<Unit>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(AddTagToVaulItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

                if (maybeVault.IsNone)
                    return new Error(ErrorCode.NotFound, $"Запись {request.VaultItemId} не была найдена");

                var vault = maybeVault.Value;

                vault.AddTag(TagId.Create(request.TagId));

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