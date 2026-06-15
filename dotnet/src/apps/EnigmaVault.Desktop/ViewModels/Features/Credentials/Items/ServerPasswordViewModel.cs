using CommunityToolkit.Mvvm.ComponentModel;
using Crossdyne.Security.Abstractions;
using EnigmaVault.Desktop.Models.Vaults;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.ViewModels.Features.Credentials.Vault;
using Shared.Contracts.Enums;

namespace EnigmaVault.Desktop.ViewModels.Features.Credentials.Items
{
    public sealed partial class ServerPasswordViewModel(CredentialsVaultViewModel model) : CredentialItemBaseViewModel(model, VaultType.Server)
    {
        #region Расшифрованные данные
       
        [ObservableProperty]
        private string? _ipAddress;

        [ObservableProperty]
        private int? _port;

        [ObservableProperty]
        private string? _domain;

        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _rootPassword;

        [ObservableProperty]
        private string? _sshKey;

        #endregion

        public override void Decrypt(string encryptedOverView, string encryptedDetails, ICryptoServices secureData, IUserContext context)
        {
            var overview = secureData.DecryptData<OverviewPayload>(encryptedOverView, context.Dek);
            var details = secureData.DecryptData<ServerPassword>(encryptedDetails, context.Dek);

            if (details is null)
                return;

            ServiceName = overview?.ServiceName!;
            Url = overview?.Url;
            Note = overview?.Note;
            SvgCode = overview?.SvgIcon;

            IpAddress = details.IpAddress;
            Port = int.Parse(details.Port!);
            Domain = details.Domain;
            Login = details.Login;
            RootPassword = details.RootPassword;
            SshKey = details.SshKey;
        }

        public override (string EncryptedOverView, string EncryptedDetails) Encrypt(ICryptoServices secureData, IUserContext context)
        {
            var overView = new OverviewPayload(ServiceName, Url!, Note, SvgCode);
            var details = new ServerPassword(IpAddress, Port?.ToString(), Domain, Login!, RootPassword, SshKey);

            var encryptedDetails = secureData.EncryptedData(details, context.Dek);
            var encryptedOverView = secureData.EncryptedData(overView, context.Dek);

            return (encryptedOverView, encryptedDetails);
        }

        public override void Clear()
        {
            ServiceName = string.Empty;
            Url = string.Empty;
            SvgCode = string.Empty;

            IpAddress = string.Empty;
            Port = null;
            Domain = string.Empty;
            Login = string.Empty;
            RootPassword = string.Empty;
            SshKey = string.Empty;
        }
    }
}