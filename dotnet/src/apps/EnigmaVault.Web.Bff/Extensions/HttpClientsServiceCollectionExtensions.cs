
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.PasswordService.ApiClient.Clients;
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
            
            return services;
        }
    }
}