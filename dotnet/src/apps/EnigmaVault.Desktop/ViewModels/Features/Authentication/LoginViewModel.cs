using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crossdyne.Security.Abstractions;
using Crossdyne.Security.Configuration;
using EnigmaVault.Authentication.Client.HttpClients;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Models;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.Requests.Authentication;
using System.Security.Cryptography;
using System.Windows;

namespace EnigmaVault.Desktop.ViewModels.Features.Authentication
{
    internal sealed partial class LoginViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IAuthService authService,
        IUserManagementService userManagementService,
        IUserContext userContext,
        ITokenManager tokenManager,
        IKeyManager keyManager,
        ISrpClient srpClient,
        IKeyDerivationService keyDerivationService,
        ICryptoServices cryptoServices) : BaseViewModel
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _authLogin = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _authPassword = string.Empty;

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task Login()
        {
            #region Конфигурация

            var cryptoProfile = CryptoProfileRegistry.GetProfile(CryptoVersion.V1);

            var srpProfile = SrpProfileRegistry.GetProfile(SrpGroup.Rfc5054_3072);
            var srpContext = SrpContext.FromOptions(srpProfile.Options);

            var normalizeLogin = AuthLogin.ToLower();

            #endregion

            var challengeResult = await authService.GetSrpChallenge(new SrpChallengeRequest(normalizeLogin));

            if (challengeResult.IsFailure)
            {
                MessageBox.Show($"challengeResult: {challengeResult.StringMessage}");
                return;
            }

            var (A, M1, S) = srpClient.GenerateSrpProof(normalizeLogin, AuthPassword, challengeResult.Value.Salt, challengeResult.Value.B, srpContext);

            var verifierResult = await authService.VerifySrpProof(new SrpVerifyRequest(normalizeLogin, A, M1));

            if (verifierResult.IsFailure)
            {
                MessageBox.Show($"verifierResult: {verifierResult.StringMessage}");
                return;
            }

            if (string.IsNullOrWhiteSpace(verifierResult.Value.M2))
            {
                MessageBox.Show("Ошибка аутентификации, возникла проблема на стороне сервера.");
                return;
            }

            var isServerValid = srpClient.VerifyServerM2(A, M1, S, verifierResult.Value.M2, srpContext);

            if (!isServerValid)
            {
                MessageBox.Show("Подлинность сервера не получилось подтвердить");
                return;
            }

            #region Дек

            var userPublicInfo = await userManagementService.GetPublicEncryptionInfo(normalizeLogin);

            if (userPublicInfo.IsFailure)
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            var publicInfo = userPublicInfo.Value;
            byte[] salt = Convert.FromBase64String(publicInfo.ClientSalt);

            var (kek, _) = keyDerivationService.DeriveKeysFromPassword(normalizeLogin, AuthPassword, salt, cryptoProfile.KdfOptions);

            byte[]? dek;

            try
            {
                var dekBase64 = cryptoServices.DecryptData<string>(publicInfo.EncryptedDek, kek, cryptoProfile.AesGcmOptions);
                dek = Convert.FromBase64String(dekBase64!);
            }
            catch (CryptographicException)
            {
                MessageBox.Show("Неверный пароль (не удалось расшифровать ключ)!");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            #endregion

            tokenManager.SaveTokens(new AccessData(verifierResult.Value!.AccessToken, verifierResult.Value!.RefreshToken));
            keyManager.SaveKey(dek!);

            var userInfoResult = await userManagementService.Me(verifierResult.Value.AccessToken);

            if (userInfoResult.IsFailure)
            {
                MessageBox.Show(userInfoResult.StringMessage);
                return;
            }

            userContext.UpdateUserInfo(new UserInfo(userInfoResult.Value!.Id, userInfoResult.Value.Login));
            userContext.UpdateTokens(new AccessData(verifierResult.Value.AccessToken, verifierResult.Value.RefreshToken));
            userContext.UpdateDek(dek!);

            Array.Clear(kek, 0, kek.Length);

            windowNavigation.Open(WindowsName.MainWindow);
            windowNavigation.Close(WindowsName.AuthenticationWindow);
            pageNavigation.Navigate(PagesName.Password, FramesName.MainFrame);
        }

        private bool CanLogin() => !string.IsNullOrWhiteSpace(AuthLogin) && !string.IsNullOrWhiteSpace(AuthPassword);

        [RelayCommand]
        private void LoginWithoutAuth()
        {
            windowNavigation.Open(WindowsName.MainWindow);
            windowNavigation.Close(WindowsName.AuthenticationWindow);
        }
    }
}