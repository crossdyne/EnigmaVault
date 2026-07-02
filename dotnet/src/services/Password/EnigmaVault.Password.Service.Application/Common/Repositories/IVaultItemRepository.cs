using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Password.Service.Domain.Models;

namespace EnigmaVault.Password.Service.Application.Common.Repositories
{
    public interface IVaultItemRepository
    {
        Task AddAsync(VaultItem vaultItem, CancellationToken clt);
        Task<Maybe<VaultItem>> GetAsync(Guid id, Guid UserId, CancellationToken clt);
        void Remove(VaultItem vaultItem);
    }
}