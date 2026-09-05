using EnigmaVault.Secret.Service.Application.Common;
using EnigmaVault.Secret.Service.Infrastructure.Persistence.Contexts;

namespace EnigmaVault.Secret.Service.Infrastructure.Persistence
{
    internal class UnitOfWork(EnigmaContext context) : IUnitOfWork
    {
        private readonly EnigmaContext _context = context;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}