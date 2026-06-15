using CommunityToolkit.Mvvm.ComponentModel;
using Crossdyne.Security.Abstractions;
using EnigmaVault.Desktop.Models.Vaults;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.ViewModels.Features.Credentials.Vault;
using Shared.Contracts.Enums;

namespace EnigmaVault.Desktop.ViewModels.Features.Credentials.Items
{
    public sealed partial class ApiKeyViewModel(CredentialsVaultViewModel model) : CredentialItemBaseViewModel(model, VaultType.ApiKey)
    {
        #region Расшифрованные данные

        [ObservableProperty]
        private string? _apiKey;

        [ObservableProperty]
        private string? _baseUrl;

        [ObservableProperty]
        private string? _clientId;

        [ObservableProperty]
        private string? _clientSecret;

        [ObservableProperty]
        private DateTime? _expirationDate;

        [ObservableProperty]
        private string? _environment;

        [ObservableProperty]
        private string? _scope;

        #endregion

        public override void Decrypt(string encryptedOverView, string encryptedDetails, ICryptoServices secureData, IUserContext context)
        {
            var overview = secureData.DecryptData<OverviewPayload>(encryptedOverView, context.Dek);
            var details = secureData.DecryptData<ApiKey>(encryptedDetails, context.Dek);

            if (details is null)
                return;

            ServiceName = overview?.ServiceName!;
            Url = overview?.Url;
            Note = overview?.Note;
            SvgCode = overview?.SvgIcon;


            ApiKey = details.Key;
            BaseUrl = details.BaseUrl;
            ClientId = details.ClientId;
            ClientSecret = details.ClientSecret;
            Environment = details.Environment;
            Scope = details.Scope;

            if (ExpirationDate is not null)
                ExpirationDate = DateTime.Parse(details.ExpirationDate!);
        }

        public override (string EncryptedOverView, string EncryptedDetails) Encrypt(ICryptoServices secureData, IUserContext context)
        {
            var overView = new OverviewPayload(ServiceName, Url!, Note, SvgCode);
            var details = new ApiKey(ApiKey, BaseUrl, ClientId, ClientSecret, ExpirationDate?.ToString("o"), Environment, Scope);

            var encryptedDetails = secureData.EncryptedData(details, context.Dek);
            var overviewEncrypted = secureData.EncryptedData(overView, context.Dek);

            return (overviewEncrypted, encryptedDetails);
        }

        public override void Clear()
        {
            ServiceName = string.Empty;
            Url = string.Empty;
            SvgCode = string.Empty;

            ApiKey = string.Empty;
            BaseUrl = string.Empty;
            ClientId = string.Empty;
            ClientSecret = string.Empty;
            ExpirationDate = null;
            Environment = string.Empty;
            Scope = string.Empty;
        }
    }
}