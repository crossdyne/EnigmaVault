using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Application.Common.Repositories;
using EnigmaVault.Password.Service.Infrastructure.Persistence.Contexts;
using EnigmaVault.Password.Service.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnigmaVault.Password.Service.Infrastructure.Ioc
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<EnigmaContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<EnigmaContext>());
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<EnigmaContext>());

            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IVaultItemRepository, VaultItemRepository>();

            return services;
        }
    }
}