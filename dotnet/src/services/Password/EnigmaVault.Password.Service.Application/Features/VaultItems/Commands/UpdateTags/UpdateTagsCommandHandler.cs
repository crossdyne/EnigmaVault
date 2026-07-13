using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Domain.Models;
using EnigmaVault.Password.Service.Domain.ValueObjects.Tag;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.UpdateTags
{
    public sealed class UpdateTagsCommandHandler(
        IApplicationDbContext context,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateTagsCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(UpdateTagsCommand request, CancellationToken cancellationToken)
        {
            var vault = await context.Set<VaultItem>().FirstOrDefaultAsync(v => v.Id == request.VaultId && v.UserId == request.UserId, cancellationToken);

            Console.WriteLine("Пошел хендлер");

            if (vault == null)
                return new Error(ErrorCode.NotFound, "Запись не найдена, возможно она была удалена");

            vault.SetTags(request.TagIds.Select(t => TagId.Create(t)));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}