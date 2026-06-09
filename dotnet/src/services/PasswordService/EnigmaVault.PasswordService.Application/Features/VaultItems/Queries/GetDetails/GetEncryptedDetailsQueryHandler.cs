using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Queries.GetDetails
{
    public sealed class GetEncryptedDetailsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetEncryptedDetailsQuery, Result<string>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result<string>> Handle(GetEncryptedDetailsQuery request, CancellationToken cancellationToken)
        {
            var vault = await _context.Set<VaultItem>().FirstOrDefaultAsync(v => v.UserId == request.UserId && v.Id == request.VaultItemId, cancellationToken);

            if (vault is null)
                return Error.NotFound("EncryptedDetails", request.VaultItemId);

            return Convert.ToBase64String(vault.EncryptedDetails);
        }
    }
}