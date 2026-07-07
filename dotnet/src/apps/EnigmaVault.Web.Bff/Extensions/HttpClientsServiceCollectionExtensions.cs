using EnigmaVault.AssetsService.Client.Clients;
using EnigmaVault.Authentication.Client.HttpClients;
using EnigmaVault.FileService.Client.Clients;
using EnigmaVault.PasswordService.Client.Clients;
using EnigmaVault.Web.Bff.Handlers;

namespace EnigmaVault.Web.Bff.Extensions
{
    public static class HttpClientsServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = new Uri(configuration["Urls:AuthService"]!));
            
            var passwordServiceName = "PasswordService";
            services.AddHttpClient(passwordServiceName, client => client.BaseAddress = new Uri(configuration["Urls:PasswordService"]!)).AddHttpMessageHandler<AccessTokenHandler>();
            services.AddHttpClient<ITagService, TagService>(passwordServiceName);

            var assetsService = "AssetsService";
            services.AddHttpClient(assetsService, client => client.BaseAddress = new Uri(configuration["Urls:AssetsService"]!)).AddHttpMessageHandler<AccessTokenHandler>();
            services.AddHttpClient<IAssetCategoryClient, AssetCategoryClient>(assetsService);
            services.AddHttpClient<IAssetClient, AssetClient>(assetsService);

            services.AddHttpClient<IFileServiceClient, FileStorageClient>(client => client.BaseAddress = new Uri(configuration["Urls:FileService"]!));

            return services;
        }
    }
}