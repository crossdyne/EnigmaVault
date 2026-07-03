using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Application.Common.Repositories;
using EnigmaVault.Password.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Password.Service.Infrastructure.Repositories
{
    internal sealed class TagRepository(IApplicationDbContext context) : ITagRepository
    {
        private readonly IApplicationDbContext _context = context;

        public async Task AddAsync(Tag tag, CancellationToken token) => await _context.Set<Tag>().AddAsync(tag, token);

        public void Remove(Tag tag) => _context.Set<Tag>().Remove(tag);

        public async Task<Maybe<Tag>> GetAsync(Guid id, Guid UserId, CancellationToken token = default)
            => await _context.Set<Tag>().FirstOrDefaultAsync(ic => ic.Id == id && ic.UserId == UserId, token);
    }
}