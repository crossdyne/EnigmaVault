using EnigmaVault.Password.Service.Application.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EnigmaVault.Password.Service.Infrastructure.Persistence.Contexts
{
    public sealed class EnigmaContext(DbContextOptions<EnigmaContext> options) : DbContext(options), IApplicationDbContext, IUnitOfWork
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}