using Crossdyne.Security.Abstractions;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Features.Authentication;

namespace EnigmaVault.Desktop.ViewModels.Windows
{
    internal sealed partial class AuthenticationWindowViewModel : BaseWindowViewModel
    {
        private readonly IWindowNavigation _windowNavigation;
        private readonly IPageNavigation _pageNavigation;
        private readonly IUserManagementService _userManagementService;
        private readonly IUserContext _userContext;
        private readonly IAuthService _authService;
        private readonly ITokenManager _tokenManager;
        private readonly IKeyManager _keyManager;
        private readonly ISrpClient _srpClient;
        private readonly IKeyDerivationService _keyDerivationService;
        private readonly ICryptoServices _cryptoServices;

        public AuthenticationWindowViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IUserManagementService userManagementService,
        IUserContext userContext,
        IAuthService authService,
        ITokenManager tokenManager,
        IKeyManager keyManager,
        ISrpClient srpClient,
        IKeyDerivationService keyDerivationService,
        ICryptoServices cryptoServices) : base(windowNavigation, pageNavigation)
        {
            _windowNavigation = windowNavigation;
            _pageNavigation = pageNavigation;
            _userManagementService = userManagementService;
            _userContext = userContext;
            _authService = authService;
            _tokenManager = tokenManager;
            _keyManager = keyManager;
            _srpClient = srpClient;
            _keyDerivationService = keyDerivationService;
            _cryptoServices = cryptoServices;

            Login = new(_windowNavigation, _pageNavigation, _authService, _userManagementService, _userContext, _tokenManager, _keyManager, _srpClient, _keyDerivationService, _cryptoServices);
        }

        public LoginViewModel Login { get; }
    }
}