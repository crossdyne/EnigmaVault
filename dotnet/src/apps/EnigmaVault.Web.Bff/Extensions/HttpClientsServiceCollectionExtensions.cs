using System.Net;
using EnigmaVault.AssetsService.Client;
using EnigmaVault.Authentication.Client;
using EnigmaVault.FileService.Client;
using EnigmaVault.PasswordService.Client.Clients;
using EnigmaVault.Web.Bff.Handlers;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Shared.Contracts.AssetsService.Clients;
using Shared.Contracts.AuthenticationService.Clients;
using Shared.Contracts.FileService.Clients;
using Shared.Contracts.PasswordService.Clients;
using Shared.Contracts.UserManagementService.Clients;

namespace EnigmaVault.Web.Bff.Extensions
{
    public static class HttpClientsServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = new Uri(configuration["Urls:AuthService"]!))
                .AddCustomResilienceHandler(maxAttempts: 2, baseDelay: TimeSpan.FromSeconds(1));

            services.AddHttpClient<IUserManagementService, UserManagementService>(client => client.BaseAddress = new Uri(configuration["Urls:UserManagementService"]!))
                .AddHttpMessageHandler<AccessTokenHandler>()
                .AddCustomResilienceHandler();

            var passwordServiceName = "PasswordService";
            services.AddHttpClient(passwordServiceName, client => client.BaseAddress = new Uri(configuration["Urls:PasswordService"]!))
                .AddHttpMessageHandler<AccessTokenHandler>()
                .AddCustomResilienceHandler();

            services.AddHttpClient<ITagService, TagService>(passwordServiceName);
            services.AddHttpClient<IVaultService, VaultService>(passwordServiceName);

            var assetsService = "AssetsService";
            services.AddHttpClient(assetsService, client => client.BaseAddress = new Uri(configuration["Urls:AssetsService"]!))
                .AddHttpMessageHandler<AccessTokenHandler>()
                .AddCustomResilienceHandler();

            services.AddHttpClient<IAssetCategoryClient, AssetCategoryClient>(assetsService);
            services.AddHttpClient<IAssetClient, AssetClient>(assetsService);

            services.AddHttpClient<IFileServiceClient, FileStorageClient>(client => client.BaseAddress = new Uri(configuration["Urls:FileService"]!))
                .AddResilienceHandler("file-retry", b =>
                {
                    b.AddRetry(new HttpRetryStrategyOptions
                    {
                        MaxRetryAttempts = 2,
                        Delay = TimeSpan.FromSeconds(2),
                        BackoffType = DelayBackoffType.Exponential,
                        UseJitter = true,
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .HandleResult(r => r.StatusCode == HttpStatusCode.ServiceUnavailable || r.StatusCode == HttpStatusCode.GatewayTimeout)
                    });
                });

            return services;
        }
        
        private static IHttpClientBuilder AddCustomResilienceHandler(this IHttpClientBuilder builder, int maxAttempts = 3, TimeSpan? baseDelay = null)
        {
            var delay = baseDelay ?? TimeSpan.FromMilliseconds(500);

            builder.AddResilienceHandler("standard-retry", b =>
            {
                b.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = maxAttempts,
                    Delay = delay,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>() 
                        .HandleResult(r => r.StatusCode == HttpStatusCode.TooManyRequests ||
                                           r.StatusCode == HttpStatusCode.BadGateway ||
                                           r.StatusCode == HttpStatusCode.ServiceUnavailable ||
                                           r.StatusCode == HttpStatusCode.GatewayTimeout)
                });
            });

            return builder;
        }
    }
}