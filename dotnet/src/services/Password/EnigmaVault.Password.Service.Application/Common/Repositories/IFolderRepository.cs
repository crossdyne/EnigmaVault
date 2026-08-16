using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Password.Service.Domain.Models;
using EnigmaVault.Password.Service.Domain.ValueObjects.User;

namespace EnigmaVault.Password.Service.Application.Common.Repositories
{
    public interface IFolderRepository
    {
        Task AddAsync(Folder folder, CancellationToken token);
        Task<Maybe<Folder>> GetAsync(Guid id, Guid UserId, CancellationToken token = default);
        void Remove(Folder folder);
        Task<bool> Exist(string name, Guid userId, Guid? parentFolderId, CancellationToken token);
        Task<int> RemoveAllAsync(UserId userId);
    }
}