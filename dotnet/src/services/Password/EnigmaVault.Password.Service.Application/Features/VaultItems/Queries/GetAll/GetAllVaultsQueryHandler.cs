using AutoMapper;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Queries.GetAll
{
    public sealed class GetAllVaultsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllVaultsQuery, Result<List<EncryptedVaultResponse>>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<EncryptedVaultResponse>>> Handle(GetAllVaultsQuery request, CancellationToken cancellationToken)
        {
            var vaultItems = await _context.Set<VaultItem>()
                .AsNoTracking()
                .Where(v => v.UserId == request.UserId)
                .Select(x => new
                {
                    x.Id,
                    x.PasswordType,
                    x.DateAdded,
                    x.DateUpdated,
                    x.DeletedAt,
                    x.IsFavorite,
                    x.IsArchive,
                    x.IsInTrash,
                    x.EncryptedOverview,
                    x.EncryptedDetails,
                    TagValues = x.Tags,
                    x.IconId
                })
                .ToListAsync(cancellationToken);

            var response = vaultItems.Select(x => new EncryptedVaultResponse(
                x.Id.ToString(),
                x.PasswordType.ToString(),
                x.DateAdded,
                x.DateUpdated,
                x.DeletedAt,
                x.IsFavorite,
                x.IsArchive,
                x.IsInTrash,
                x.EncryptedOverview,
                x.EncryptedDetails,
                [.. x.TagValues.Select(v => v.ToString())],
                x.IconId.ToString()
            )).ToList();

            return response;
        }
    }
}