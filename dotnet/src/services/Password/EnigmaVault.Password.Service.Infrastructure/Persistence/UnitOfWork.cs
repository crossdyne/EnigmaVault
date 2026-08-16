using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Infrastructure.Persistence.Contexts;

namespace EnigmaVault.Password.Service.Infrastructure.Persistence
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