using Crossdyne.Toolkit.Primitives;
using EnigmaVault.PasswordService.Domain.Models;

namespace EnigmaVault.PasswordService.Application.Common.Repositories
{
    public interface IVaultItemRepository
    {
        Task AddAsync(VaultItem vaultItem, CancellationToken clt);
        Task<Maybe<VaultItem>> GetAsync(Guid id, Guid UserId, CancellationToken clt);
        void Remove(VaultItem vaultItem);
    }
}