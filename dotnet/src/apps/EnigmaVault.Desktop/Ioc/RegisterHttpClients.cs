using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Handlers;
using EnigmaVault.PasswordService.ApiClient.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnigmaVault.Desktop.Ioc
{
    public static class RegisterHttpClients
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<RefreshTokenHandler>();

            string? authServiceApiUrl = configuration.GetValue<string>("BaseAuthServiceUrl");
            const string authServiceApiClientName = "AuthApiClient";
            services.AddHttpClient(authServiceApiClientName, client => client.BaseAddress = new Uri(authServiceApiUrl!));
            services.AddHttpClient<IAuthService, AuthService>(authServiceApiClientName);

            string? userManagementServiceApiUrl = configuration.GetValue<string>("BaseUserManagementServiceUrl");
            const string userManagementServiceApiClientName = "UserManagementApiClient";
            services.AddHttpClient(userManagementServiceApiClientName, client => client.BaseAddress = new Uri(userManagementServiceApiUrl!));
            services.AddHttpClient<IUserManagementService, UserManagementService>(userManagementServiceApiClientName);

            string? passwordServiceApiUrl = configuration.GetValue<string>("BasePasswordServiceUrl");
            const string passwordServiceApiClientName = "PasswordApiClient";
            services.AddHttpClient(passwordServiceApiClientName, client => client.BaseAddress = new Uri(passwordServiceApiUrl!)).AddHttpMessageHandler<RefreshTokenHandler>();
            services.AddHttpClient<ITagService, TagService>(passwordServiceApiClientName);
            services.AddHttpClient<IIconCategoryService, IconCategoryService>(passwordServiceApiClientName);
            services.AddHttpClient<IIconService, IconService>(passwordServiceApiClientName);
            services.AddHttpClient<IVaultService, VaultService>(passwordServiceApiClientName);

            return services;
        }
    }
}