
using EnigmaVault.Authentication.ApiClient.HttpClients;

namespace EnigmaVault.Web.Bff.Extensions
{
    public static class HttpClientsServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var authBaseUrl = configuration["Urls:AuthServicesBase"];
            string authenticationServices = "AuthenticationServices";
            services.AddHttpClient<IAuthService, AuthService>(authenticationServices, client => client.BaseAddress = new Uri(authBaseUrl!));
            
            return services;
        }
    }
}