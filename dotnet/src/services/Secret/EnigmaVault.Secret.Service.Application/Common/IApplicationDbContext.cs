using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Service.Application.Common
{
    public interface IApplicationDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}