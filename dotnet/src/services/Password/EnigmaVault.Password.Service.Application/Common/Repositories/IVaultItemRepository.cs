using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Password.Service.Domain.Models;
using EnigmaVault.Password.Service.Domain.ValueObjects.User;

namespace EnigmaVault.Password.Service.Application.Common.Repositories
{
    public interface IVaultItemRepository
    {
        Task AddAsync(VaultItem vaultItem, CancellationToken clt);
        Task<Maybe<VaultItem>> GetAsync(Guid id, Guid UserId, CancellationToken clt);
        void Remove(VaultItem vaultItem);
        Task<int> RemoveAllAsync(UserId userId, DateTime? eventTimeUtc);
    }
}