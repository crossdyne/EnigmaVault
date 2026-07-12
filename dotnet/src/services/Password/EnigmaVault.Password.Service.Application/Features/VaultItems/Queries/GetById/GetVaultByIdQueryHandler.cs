using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Queries.GetById
{
    public sealed class GetVaultByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetVaultByIdQuery, Result<EncryptedVaultResponse>>
    {
        public async Task<Result<EncryptedVaultResponse>> Handle(GetVaultByIdQuery request, CancellationToken cancellationToken)
        {
            var vault = await context.Set<VaultItem>().AsNoTracking().FirstOrDefaultAsync(v => v.UserId == request.UserId && v.Id == request.Id);

            if (vault == null)
                return new Error(ErrorCode.NotFound, "Не найдено записи, возможно она уже удалена");

            return new EncryptedVaultResponse(
                vault.Id.ToString(), 
                vault.PasswordType.ToString(), 
                vault.DateAdded, 
                vault.DateUpdated, 
                vault.DeletedAt, 
                vault.IsFavorite, 
                vault.IsArchive, 
                vault.IsInTrash, 
                vault.EncryptedOverview, 
                vault.EncryptedDetails, 
                [.. vault.Tags.Select(t => t.ToString())], 
                vault.IconId.ToString());
        }
    }
}