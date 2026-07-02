using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Password.Service.Application.Common
{
    public interface IApplicationDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}