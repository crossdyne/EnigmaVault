using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Service.Application.Common;
using EnigmaVault.Secret.Service.Application.Common.Repositories;
using EnigmaVault.Secret.Service.Domain.Models;
using EnigmaVault.Secret.Service.Domain.ValueObjects.Password;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.VaultItems.Commands.ChangeIcon
{
    public sealed class ChangeIconCommandHandler(
        IVaultItemRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<ChangeIconCommand, Result<DateTime>>
    {
        public async Task<Result<DateTime>> Handle(ChangeIconCommand request, CancellationToken cancellationToken)
        {
            Maybe<VaultItem> maybe = await repository.GetAsync(request.VaultId, request.UseId, cancellationToken);

            if (maybe.IsNone)
                return new Error(ErrorCode.NotFound, "Данная запись не найдена, возможно она была удалена");

            VaultItem vault = maybe.Value;

            vault.SetIcon(IconId.Create(request.IconId));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return vault.DateUpdated!;
        }
    }
}