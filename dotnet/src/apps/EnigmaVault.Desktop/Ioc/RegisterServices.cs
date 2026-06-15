using Crossdyne.Security.Abstractions;
using Crossdyne.Security.Cryptography;
using Crossdyne.Security.Srp.Client;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.Initializers;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.Services.WindowNavigation;
using Microsoft.Extensions.DependencyInjection;

namespace EnigmaVault.Desktop.Ioc
{
    public static class RegisterServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IWindowNavigation, WindowNavigation>();
            services.AddSingleton<IPageNavigation, PageNavigation>();
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<IKeyManager, KeyManager>();
            services.AddSingleton<IApplicationInitializer, ApplicationInitializer>();
            services.AddSingleton<IUserContext, UserContext>();
            services.AddSingleton<ISrpClient, SrpClientService>();
            services.AddSingleton<IKeyDerivationService, KeyDerivationService>();
            services.AddSingleton<ICryptoServices, CryptoService>();
            services.AddSingleton<IAuthenticationStateService, AuthenticationStateService>();

            return services;
        }
    }
}