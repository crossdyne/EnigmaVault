using EnigmaVault.Secret.Service.Infrastructure.Persistence.Constants;
using EnigmaVault.Secret.Service.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Service.Integration.Tests.Contexts
{
    public class TestEnigmaContext(DbContextOptions<EnigmaContext> options) : EnigmaContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnigmaContext).Assembly);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.GetCollation() == PostgresConstants.COLLATION_NAME)
                    {
                        property.SetCollation("NOCASE");
                    }
                }
            }
        }
    }
}