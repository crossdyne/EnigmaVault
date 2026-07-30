using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Domain.Models;
using EnigmaVault.Password.Service.Domain.ValueObjects.Password;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.ChangeIcon
{
    public sealed class ChangeIconCommandHandler(
        IApplicationDbContext context,
        IUnitOfWork unitOfWork) : IRequestHandler<ChangeIconCommand, Result<DateTime>>
    {
        public async Task<Result<DateTime>> Handle(ChangeIconCommand request, CancellationToken cancellationToken)
        {
            var vault = await context.Set<VaultItem>().FirstOrDefaultAsync(v => v.UserId == request.UseId && v.Id == request.VaultId);

            if (vault == null)
                return new Error(ErrorCode.NotFound, "Данная запись не найдена, возможно она была удалена");

            vault.SetIcon(IconId.Create(request.IconId));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return vault.DateUpdated!;
        }
    }
}