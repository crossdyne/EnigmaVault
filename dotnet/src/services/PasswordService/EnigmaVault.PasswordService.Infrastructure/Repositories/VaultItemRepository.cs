using Crossdyne.Toolkit.Primitives;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.PasswordService.Infrastructure.Repositories
{
    public sealed class VaultItemRepository(IApplicationDbContext context) : IVaultItemRepository
    {
        private readonly IApplicationDbContext _context = context;

        public async Task AddAsync(VaultItem vaultItem, CancellationToken clt) => await _context.Set<VaultItem>().AddAsync(vaultItem, clt);

        public void Remove(VaultItem vaultItem) => _context.Set<VaultItem>().Remove(vaultItem);

        public async Task<Maybe<VaultItem>> GetAsync(Guid id, Guid UserId, CancellationToken clt)
            => await _context.Set<VaultItem>().FirstOrDefaultAsync(v => v.Id == id && v.UserId == UserId, clt);
    }
}