using Crossdyne.Toolkit.Primitives;
using EnigmaVault.PasswordService.Domain.Models;

namespace EnigmaVault.PasswordService.Application.Common.Repositories
{
    public interface IFolderRepository
    {
        Task AddAsync(Folder folder, CancellationToken token);
        Task<Maybe<Folder>> GetAsync(Guid id, Guid UserId, CancellationToken token = default);
        void Remove(Folder folder);
        Task<bool> Exist(string name, Guid userId, Guid? parentFolderId, CancellationToken token);
    }
}