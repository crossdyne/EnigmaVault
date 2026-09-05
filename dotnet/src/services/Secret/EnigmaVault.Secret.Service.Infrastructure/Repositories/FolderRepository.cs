using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Secret.Service.Application.Common;
using EnigmaVault.Secret.Service.Application.Common.Repositories;
using EnigmaVault.Secret.Service.Domain.Models;
using EnigmaVault.Secret.Service.Domain.ValueObjects.User;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Service.Infrastructure.Repositories
{
    internal sealed class FolderRepository(IApplicationDbContext context) : IFolderRepository
    {
        private readonly IApplicationDbContext _context = context;

        public async Task AddAsync(Folder folder, CancellationToken token) => await _context.Set<Folder>().AddAsync(folder, token);

        public void Remove(Folder folder) => _context.Set<Folder>().Remove(folder);

        public async Task<Maybe<Folder>> GetAsync(Guid id, Guid UserId, CancellationToken token = default)
            => await _context.Set<Folder>().FirstOrDefaultAsync(f => f.Id == id && f.UserId == UserId, token);

        public async Task<bool> Exist(string name, Guid userId, Guid? parentFolderId, CancellationToken token)
            => await _context.Set<Folder>().AnyAsync(f =>f.UserId == userId && f.ParentFolderId == parentFolderId && f.FolderName == name, token);

        public async Task<int> RemoveAllAsync(UserId userId)
        {
            List<Folder> folders = await _context.Set<Folder>().Where(f => f.UserId == userId).ToListAsync();
            _context.Set<Folder>().RemoveRange(folders);

            return folders.Count;
        }
    }
}