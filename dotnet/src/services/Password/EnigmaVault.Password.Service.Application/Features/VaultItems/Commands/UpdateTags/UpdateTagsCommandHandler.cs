using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Domain.Models;
using EnigmaVault.Password.Service.Domain.ValueObjects.Tag;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.UpdateTags
{
    public sealed class UpdateTagsCommandHandler(
        IApplicationDbContext context,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateTagsCommand, Result<DateTime>>
    {
        public async Task<Result<DateTime>> Handle(UpdateTagsCommand request, CancellationToken cancellationToken)
        {
            var vault = await context.Set<VaultItem>()
                .Include(v => v.Tags)
                .FirstOrDefaultAsync(v => v.Id == request.VaultId && v.UserId == request.UserId, cancellationToken);

            if (vault == null)
                return new Error(ErrorCode.NotFound, "Запись не найдена, возможно она была удалена");

            vault.SetTags(request.TagIds.Select(id => TagId.Create(id)));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return vault.DateUpdated!;
        }
    }
}