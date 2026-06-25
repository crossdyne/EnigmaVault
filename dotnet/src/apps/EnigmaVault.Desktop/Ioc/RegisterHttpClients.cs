using EnigmaVault.AssetsService.ApiClient.Clients;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Handlers;
using EnigmaVault.FileService.ApiClient.Clients;
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
            services.AddHttpClient<IVaultService, VaultService>(passwordServiceApiClientName);

            string? assetsServiceApiUrl = configuration.GetValue<string>("BaseAssetsServiceUrl");
            const string assetsServiceApiClientName = "AssetsApiClient";
            services.AddHttpClient(assetsServiceApiClientName, client => client.BaseAddress = new Uri(assetsServiceApiUrl!)).AddHttpMessageHandler<RefreshTokenHandler>();
            services.AddHttpClient<IAssetClient, AssetClient>(assetsServiceApiClientName);
            services.AddHttpClient<IAssetCategoryClient, AssetCategoryClient>(assetsServiceApiClientName);

            string? fileServiceApiUrl = configuration.GetValue<string>("BaseFileServiceUrl");
            services.AddHttpClient<IFileServiceClient, FileStorageClient>(client => client.BaseAddress = new Uri(fileServiceApiUrl!));

            return services;
        }
    }
}