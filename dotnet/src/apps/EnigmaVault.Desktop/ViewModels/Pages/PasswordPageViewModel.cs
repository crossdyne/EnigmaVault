using Common.Core.Results;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crossdyne.Security.Abstractions;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Common.Assets;
using EnigmaVault.Desktop.ViewModels.Common.Controls;
using EnigmaVault.Desktop.ViewModels.Common.Organization;
using EnigmaVault.Desktop.ViewModels.Features.Credentials.Items;
using EnigmaVault.Desktop.ViewModels.Features.Credentials.Vault;
using EnigmaVault.PasswordService.ApiClient.Clients;
using Shared.Contracts.Enums;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Pages
{
    internal sealed partial class PasswordPageViewModel : BasePageViewModel, IAsyncInitializable, IUpdatable, ISidebarController
    {
        private readonly IVaultService _vaultService;
        private readonly ITagService _tagService;
        private readonly IIconCategoryService _iconCategoryService;
        private readonly IIconService _iconService;
        private readonly IUserContext _userContext;
        private readonly ICryptoServices _cryptoServices;

        // ====================================================================================
        //                                      ИНИЦИАЛИЗАЦИЯ                                        
        // ====================================================================================

        public PasswordPageViewModel(
            IVaultService vaultService,
            ITagService tagService,
            IIconCategoryService iconCategoryService,
            IIconService iconService,
            IUserContext userContext,
            ICryptoServices cryptoServices)
        {
            _vaultService = vaultService;
            _tagService = tagService;
            _iconCategoryService = iconCategoryService;
            _iconService = iconService;
            _userContext = userContext;
            _cryptoServices = cryptoServices;

            SelectedPasswordType = PasswordTypes.FirstOrDefault();
            CurrentDisplayUserControlLeftSideMenu = UserControlsName.Tags;
            CurrentActionRightSideMenu = ActionOnData.Create;

            PasswordsView = CollectionViewSource.GetDefaultView(Passwords);
            IconView = CollectionViewSource.GetDefaultView(Icons);
            IconCategoryView = CollectionViewSource.GetDefaultView(IconCategories);

            UpdateIconsView();
            UpdateIconCategoriesView();

            ArchivesPopup = new(PopupPlacementMode.CustomRightUp, PlacementMode.Custom, () => ArchivedPasswords.Count > 0);

            ArchivedPasswords.CollectionChanged += (s, e) =>
            {
                ArchivesPopup?.UpdateCanExecute();
            };

            TrashPopup = new(PopupPlacementMode.CustomRightUp, PlacementMode.Custom, () => TrashPasswords.Count > 0);

            TrashPasswords.CollectionChanged += (s, e) =>
            {
                TrashPopup?.UpdateCanExecute();
                RestoreAllTrashCommand.NotifyCanExecuteChanged();
                EmptyTrashCommand.NotifyCanExecuteChanged();
            };

            AttachTagsPopup = new(PopupPlacementMode.CustomCenter, PlacementMode.Custom);

            CurrentTemplateTypePasswords = TemplateType.Detailed;
            SelectedGrouping = GroupingView.None;
        }

        public async Task InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            try
            {
                await Task.Delay(1111);

                await GetTags();
                await GetIconCategories();
                await GetIcons();

                await GetEncreptedOverview();

                UpdateGroupingPassword();

                IsInitialize = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка инициализации: {ex}");
            }
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {

        }

        // ====================================================================================
        //                                      КОЛЛЕКЦИИ                                        
        // ====================================================================================

        public ObservableCollection<CredentialsVaultViewModel> Passwords { get; init; } = [];
        public ICollectionView PasswordsView { get; private init; } = null!;

        public ObservableCollection<CredentialsVaultViewModel> ArchivedPasswords { get; init; } = [];
        public ObservableCollection<CredentialsVaultViewModel> TrashPasswords { get; init; } = [];
        public ObservableCollection<TagViewModel> Tags { get; init; } = [];

        public ObservableCollection<IconViewModel> Icons { get; init; } = [];
        public ICollectionView IconView { get; private set; } = null!;

        public ObservableCollection<IconCategoryViewModel> IconCategories { get; init; } = [];
        public ICollectionView IconCategoryView { get; private init; } = null!;

        public ObservableCollection<KeyValuePair<VaultType, string>> PasswordTypes { get; private set; } =
        [
            new KeyValuePair<VaultType, string>(VaultType.Password, "Пароль"),
            new KeyValuePair<VaultType, string>(VaultType.Server, "Данные сервера"),
            new KeyValuePair<VaultType, string>(VaultType.CreditCard, "Банковские карты"),
            new KeyValuePair<VaultType, string>(VaultType.ApiKey, "Апи Ключи"),
        ];

        // TODO: Реализовать в будущем.
        public ObservableCollection<KeyValuePair<SortingView, string>> Sorting { get; private set; } =
        [
            new KeyValuePair<SortingView, string>(SortingView.Ascending, "По возрастанию (от А до Я, от 0 до 9)"),
            new KeyValuePair<SortingView, string>(SortingView.Descending, "По убыванию (от Я до А, от 9 до 0)"),
        ];

        // ====================================================================================
        //                                      СВОЙСТВА                                        
        // ====================================================================================

        public ToolTipController RightToolTipController { get; } = new(ToolTipPlacement.CenterRight);
        public ToolTipController LeftToolTipController { get; } = new(ToolTipPlacement.CenterLeft);
        public ToolTipController TopToolTipController { get; } = new(ToolTipPlacement.CenterTop);
        public ToolTipController BottomToolTipController { get; } = new(ToolTipPlacement.CenterBottom);

        public PopupController PasswordMenuPopup { get; } = new(); 
        public PopupController AttachTagsPopup { get; } 
        public PopupController AddIconPopup { get; } = new(); 
        public PopupController AddIconCategoryPopup { get; } = new();
        public PopupController ArchivesPopup { get; }
        public PopupController TrashPopup { get; }

        // ================Vault=====================

        #region Свойсто: [SelectedEncryptedOverview] - Выбор зашифрованного элемента

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateVaultCommand))]
        private CredentialsVaultViewModel? _selectedEncryptedOverview;

        partial void OnSelectedEncryptedOverviewChanged(CredentialsVaultViewModel? value)
        {
            if (value is null)
                return;

            CreateViewModelForType(value.Type, value);
            SelectedPasswordType = PasswordTypes.FirstOrDefault(pt => pt.Key == value.Type);
            CurrentActionRightSideMenu = ActionOnData.View;
            SetReadOnly(CurrentActionRightSideMenu);
            SelectedCredentialItemBaseViewModel?.Decrypt(value.EncryptedOverview, value.EncryptedDetails, _cryptoServices, _userContext);
            SelectedCredentialItemBaseViewModel?.SetIcon(ConvertSvgInString(value.SvgCode!));

            foreach (var tag in Tags)
            {
                if (tag is null) continue;

                if (value.Tags.Contains(tag))
                    tag.AttachedTag();
                else
                    tag.DetatchedTag();
            }
        }

        #endregion

        #region Свойсто: [SelectedArchivedEncryptedOverview] - Выбор зашифрованного элемента в архиве

        [ObservableProperty]
        private CredentialsVaultViewModel? _selectedArchivedEncryptedOverview;

        #endregion

        #region Свойсто: [SelectedTrashEncryptedOverview] - Выбор зашифрованного элемента в корзине

        [ObservableProperty]
        private CredentialsVaultViewModel? _selectedTrashEncryptedOverview;

        #endregion

        #region Свойство: [SelectedPasswordType], Метод [OnSelectedPasswordTypeChanged]

        [ObservableProperty]
        private KeyValuePair<VaultType, string> _selectedPasswordType;

        partial void OnSelectedPasswordTypeChanged(KeyValuePair<VaultType, string> value)
        {
            CreateViewModelForType(value.Key, SelectedEncryptedOverview!);
            SetReadOnly(CurrentActionRightSideMenu);
        }

        #endregion

        #region Свойсто: [SelectedPasswordViewModel]

        [ObservableProperty]
        private CredentialItemBaseViewModel? _selectedCredentialItemBaseViewModel;

        partial void OnSelectedCredentialItemBaseViewModelChanged(CredentialItemBaseViewModel? value)
        {
            value?.SetIsReadOnly(CurrentActionRightSideMenu != ActionOnData.View);
        }

        #endregion

        #region Свойство: [SelectedGrouping] - Выбор группровки списка паролей

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetGroupingPasswordsCommand))]
        private GroupingView _selectedGrouping;

        partial void OnSelectedGroupingChanged(GroupingView value)
        {
            UpdateGroupingPassword();
        }

        #endregion

        #region Свойство: [SelectedSorting] - Выбор сортировки списка паролей

        [ObservableProperty]
        private KeyValuePair<SortingView, string> _selectedSorting;

        #endregion

        // =================Tag======================

        #region Свойства: Tags, Метод: [OnSelectedTagChanged]

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateTagCommand))]
        private string? _nameTag;

        [ObservableProperty]
        private TagViewModel? _selectedTag;

        partial void OnSelectedTagChanged(TagViewModel? value)
        {
            if (value is not null)
            {
                UpdateRed = value!.RgbColor.R.ToString();
                UpdateGreen = value!.RgbColor.G.ToString();
                UpdateBlue = value!.RgbColor.B.ToString();

                value.SetColor(Color.FromRgb(byte.Parse(UpdateRed), byte.Parse(UpdateGreen), byte.Parse(UpdateBlue)));
            }
        }

        [ObservableProperty]
        private TagViewModel? _selectedAttachedTag;

        partial void OnSelectedAttachedTagChanged(TagViewModel? value)
        {
            if (value is null)
                return;

            if (value.IsAttached)
                DetachTagToSelectedVaultCommand.Execute(value);
            else
                AttachTagToSelectedVaultCommand.Execute(value);
        }

        [ObservableProperty]
        private string _red = "255";

        [ObservableProperty]
        private string _green = "255";

        [ObservableProperty]
        private string _blue = "255";

        [ObservableProperty]
        private string _updateRed = null!;

        [ObservableProperty]
        private string _updateGreen = null!;

        [ObservableProperty]
        private string _updateBlue = null!;

        partial void OnUpdateRedChanged(string value) => UpdateSelectedTagColor();

        partial void OnUpdateGreenChanged(string value) => UpdateSelectedTagColor();

        partial void OnUpdateBlueChanged(string value) => UpdateSelectedTagColor();

        private void UpdateSelectedTagColor()
        {
            if (SelectedTag == null)
                return;

            bool redParsed = byte.TryParse(UpdateRed, out byte r);
            bool greenParsed = byte.TryParse(UpdateGreen, out byte g);
            bool blueParsed = byte.TryParse(UpdateBlue, out byte b);

            if (redParsed && greenParsed && blueParsed)
                SelectedTag?.SetColor(System.Windows.Media.Color.FromRgb(r, g, b));
        }


        #endregion

        // ================Icon======================

        #region Свойство: [SelectedIcon]

        [ObservableProperty]
        private IconViewModel? _selectedIcon;

        partial void OnSelectedIconChanged(IconViewModel? value)
        {
            if (value is null)
                return;

            if (CurrentActionRightSideMenu is ActionOnData.Create || CurrentActionRightSideMenu is ActionOnData.Update)
            {
                if (SelectedCredentialItemBaseViewModel is null)
                    return;

                string? code = SelectedIcon?.SvgCode;
                SelectedCredentialItemBaseViewModel.SvgCode = code;
                SelectedCredentialItemBaseViewModel?.SetIcon(ConvertSvgInString(code!));
            }
        }

        #endregion

        #region Свойства: SVG

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveIconCommand))]
        private string? _svgCode;

        partial void OnSvgCodeChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                SvgConvertError = string.Empty;
                Svg = null;
                return;
            }

            try
            {
                Svg = ConvertSvgInString(value);
            }
            catch (Exception ex)
            {
                SvgConvertError = ex.Message;
            }

        }

        [ObservableProperty]
        private DrawingImage? _svg;

        [ObservableProperty]
        private string? _svgConvertError;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveIconCommand))]
        private IconCategoryViewModel? _selectedIconCategory;

        #endregion

        #region Свойства: IconName

        [ObservableProperty]
        private string? _iconName;

        #endregion

        // ============IconCategory==================

        #region Свойства: IconCategories, Методы: [OnSelectedEditableCategoryChanging, OnSelectedEditableCategoryChanged, OnCategoryPropertyChanged]

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveIconCategoryCommand))]
        private string? _iconCategoryName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateIconCategortyCommand))]
        private IconCategoryViewModel? _selectedEditableCategory;

        partial void OnSelectedEditableCategoryChanging(IconCategoryViewModel? value)
        {
            if (SelectedEditableCategory is not null)
                SelectedEditableCategory.PropertyChanged -= OnCategoryPropertyChanged;
        }

        partial void OnSelectedEditableCategoryChanged(IconCategoryViewModel? value)
        {
            if (value is not null)
                value.PropertyChanged += OnCategoryPropertyChanged;

            UpdateIconCategortyCommand.NotifyCanExecuteChanged();
        }

        private void OnCategoryPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IconCategoryViewModel.HasChanges))
                UpdateIconCategortyCommand.NotifyCanExecuteChanged();
        }


        #endregion

        // ==============SideMenu====================

        #region Свойства: [CurrentDisplayUserControlLeftSideMenu, CurrentDisplayUserControlRightSideMenu] - Текущий отображаемый элемент в боковых меню

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetLeftSideMenuControlCommand))]
        private UserControlsName _currentDisplayUserControlLeftSideMenu = UserControlsName.Folders;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetRightSideMenuActionCommand))]
        private ActionOnData _currentActionRightSideMenu = ActionOnData.View;

        partial void OnCurrentActionRightSideMenuChanged(ActionOnData value)
        {
            SetReadOnly(value);

            if (value == ActionOnData.Create || value == ActionOnData.Update)
                SelectedIcon = Icons.FirstOrDefault(i => i.SvgCode == SelectedEncryptedOverview?.SvgCode);
            else
                SelectedIcon = null;
        }

        #endregion

        #region Свойство: [CurrentTemplateTypePasswords] - Текущий отображаемый темплей у списка с паролями.

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetTemplatePasswordsCommand))]
        private TemplateType _currentTemplateTypePasswords;

        #endregion

        // ====================================================================================
        //                                      КОМАНДЫ                                        
        // ====================================================================================

        // ================Vault=====================

        /*--CRUD--*/

        #region Команда [CreateVault]: Создание зашифрованного элемента

        [RelayCommand]
        public async Task CreateVault()
        {
            (string EncryptedOverView, string EncryptedDetails) = SelectedCredentialItemBaseViewModel!.Encrypt(_cryptoServices, _userContext);

            var result = await _vaultService.CreateAsync(new CreateVaultItemRequest(_userContext.Id, SelectedPasswordType.Key.ToString(), EncryptedOverView, EncryptedDetails));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            var encryptedVm = new CredentialsVaultViewModel(
                    new EncryptedVaultResponse(
                        result.Value,
                        SelectedPasswordType.Key.ToString(),
                        DateTime.Now,
                        DateUpdate: null,
                        DeletedAt: null,
                        IsFavorite: false,
                        IsArchive: false,
                        IsInTrash: false,
                        Convert.FromBase64String(EncryptedOverView),
                        Convert.FromBase64String(EncryptedDetails),
                        []),
                    _cryptoServices, 
                    _userContext.Dek,
                    Tags);

            encryptedVm.Icon = ConvertSvgInString(encryptedVm.SvgCode);

            Passwords.Add(encryptedVm);
        }

        #endregion

        #region Команда [UpdateVault]: Обноволение записи

        [RelayCommand(CanExecute = nameof(CanUpdateVault))]
        private async Task UpdateVault()
        {
            (string EncryptedOverView, string EncryptedDetails) = SelectedCredentialItemBaseViewModel!.Encrypt(_cryptoServices, _userContext);

            var result = await _vaultService.UpdateAsync(new UpdateVaultItemRequest(_userContext.Id, SelectedEncryptedOverview!.Id, EncryptedOverView, EncryptedDetails));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            SelectedEncryptedOverview!.UpdateEncrypted(EncryptedOverView, EncryptedDetails);
            SelectedEncryptedOverview!.UpdateDate(DateTime.Parse(result.Value).ToLocalTime());
            SelectedEncryptedOverview.SetIcon(SelectedIcon?.Icon);
        }

        private bool CanUpdateVault() => SelectedEncryptedOverview is not null;

        #endregion

        /*--Action--*/

        #region Команда [SetFavorite]: Измнение статуса избранного

        [RelayCommand(CanExecute = nameof(CanSetFavorite))]
        private async Task SetFavorite(CredentialsVaultViewModel model)
        {
            void SetValue(Result<Unit> result, bool condition)
            {
                if (result.IsFailure)
                {
                    MessageBox.Show($"{result.StringMessage}");
                    return;
                }

                model!.IsFavorite = condition;
            }

            if (model.IsFavorite)
            {
                var result = await _vaultService.RemoveFromFavoritesAsync(_userContext.Id, model.Id);
                SetValue(result, false);
            }
            else
            {
                var result = await _vaultService.AddToFavoritesAsync(_userContext.Id, model.Id);
                SetValue(result, true);
            }
        }

        private bool CanSetFavorite(CredentialsVaultViewModel model) => model is not null;

        #endregion

        #region Команда [SetArchive]: Измнение статуса архивации

        [RelayCommand(CanExecute = nameof(CanSetArchive))]
        private async Task SetArchive(CredentialsVaultViewModel model)
        {
            void SetValue(Result<Unit> result, bool condition)
            {
                if (result.IsFailure)
                {
                    MessageBox.Show($"{result.StringMessage}");
                    return;
                }

                model!.IsArchive = condition;

                if (condition)
                {
                    Passwords.Remove(model);
                    ArchivedPasswords.Add(model);
                }
                else
                {
                    ArchivedPasswords.Remove(model);
                    Passwords.Add(model);
                }

                PasswordMenuPopup.HideCommand.Execute(null);
            }

            if (model.IsArchive)
            {
                var result = await _vaultService.UnArchiveAsync(_userContext.Id, model.Id);
                SetValue(result, false);

                if (ArchivedPasswords.Count <= 0)
                    ArchivesPopup.HideCommand.Execute(null);
            }
            else
            {
                var result = await _vaultService.ArchiveAsync(_userContext.Id, model.Id);
                SetValue(result, true);
            }
        }

        private bool CanSetArchive(CredentialsVaultViewModel model) => model is not null;

        #endregion

        #region Команда [AttachTagToSelectedVaultCommand]: Присоединяет тэг к выбранному зашифрованному элементу

        [RelayCommand]
        private async Task AttachTagToSelectedVault(TagViewModel tag)
        {
            if (SelectedEncryptedOverview is null)
                return;

            var result = await _vaultService.AddTagAsync(_userContext.Id, SelectedEncryptedOverview.Id, tag.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            SelectedEncryptedOverview.AddTag(tag.Id);

            if (tag.IsAttached)
                tag.DetatchedTag();
            else
                tag.AttachedTag();
        }

        [RelayCommand]
        private async Task DetachTagToSelectedVault(TagViewModel tag)
        {
            if (SelectedEncryptedOverview is null)
                return;

            var result = await _vaultService.RemoveTagAsync(_userContext.Id, SelectedEncryptedOverview.Id, tag.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            SelectedEncryptedOverview.RemoveTag(tag.Id);
            tag.DetatchedTag();
        }

        #endregion

        /*--Trash--*/

        #region Команда [MoveToTrashCommand]: Переносит запись в карзину (Мягкое удаление)

        [RelayCommand(CanExecute = nameof(CanMoveToTrash))]
        private async Task MoveToTrash(CredentialsVaultViewModel model)
        {
            var result = await _vaultService.MoveToTrashAsync(_userContext.Id, model.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            model.DeletedAt = result.Value;

            Passwords.Remove(model);
            TrashPasswords.Add(model);
            PasswordMenuPopup.HideCommand.Execute(null);
        }

        private bool CanMoveToTrash(CredentialsVaultViewModel model) => model is not null;

        #endregion

        #region Команда [RestoreTrashCommand]: Восстанавливает запись из корзины

        [RelayCommand(CanExecute = nameof(CanRestoreTrash))]
        private async Task RestoreTrash(CredentialsVaultViewModel model)
        {
            var result = await _vaultService.RestoreFromTrashAsync(_userContext.Id, model.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            model.DeletedAt = null;

            TrashPasswords.Remove(model);
            Passwords.Add(model);

            if (TrashPasswords.Count <= 0)
                TrashPopup.HideCommand.Execute(null);
        }

        private bool CanRestoreTrash(CredentialsVaultViewModel model) => model is not null;

        #endregion

        #region Команда [RestoreAllTrashCommand] : Востановить все записи из корзины

        [RelayCommand(CanExecute = nameof(CanRestoreAllTrash))]
        private async Task RestoreAllTrash()
        {
            if (MessageBox.Show($"Вы точно хотите востановить все записи в кол-ве {TrashPasswords.Count}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            var result = await _vaultService.RestoreAllFromTrashAsync(_userContext.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            foreach (var vault in TrashPasswords.ToList())
            {
                TrashPasswords.Remove(vault);
                vault.IsInTrash = false;
                Passwords.Add(vault);
            }

            TrashPopup.HideCommand.Execute(null);
        }

        private bool CanRestoreAllTrash() => TrashPasswords.Count > 0;

        #endregion

        #region Команда [EmptyTrashCommand] : Очистка корзины

        [RelayCommand(CanExecute = nameof(CanEmptyTrash))]
        private async Task EmptyTrash()
        {
            if (MessageBox.Show($"Вы точно хотите удалить все записи в кол-ве {TrashPasswords.Count}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            var result = await _vaultService.EmptyTrashAsync(_userContext.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            TrashPasswords.Clear();

            TrashPopup.HideCommand.Execute(null);
        }

        private bool CanEmptyTrash() => TrashPasswords.Count > 0;

        #endregion

        /*--Template--*/

        #region Команда [SetTemplatePasswordsCommand]: Выбор текущего темплейта у списка с паролями

        [RelayCommand(CanExecute = nameof(CanSetTemplatePasswords))]
        private void SetTemplatePasswords(TemplateType type) => CurrentTemplateTypePasswords = type;

        private bool CanSetTemplatePasswords(TemplateType type) => type != CurrentTemplateTypePasswords;

        #endregion

        #region Команда [SetGroupingPasswordsCommand]: Выбор текущей группировки у списка с паролями

        [RelayCommand(CanExecute = nameof(CanSetGroupingPasswords))]
        private void SetGroupingPasswords(GroupingView type) => SelectedGrouping = type;

        private bool CanSetGroupingPasswords(GroupingView type) => type != SelectedGrouping;

        #endregion

        /*--PopupMenagement--*/

        #region Команда [SelectAndShowPasswordMenuPopup]: Отвечает за выбор элемента списка паролей при открытие контекстного меню 

        [RelayCommand]
        private void SelectAndShowPasswordMenuPopup(CredentialsVaultViewModel password)
        {
            if (password is null) return;

            SelectedEncryptedOverview = password;

            PasswordMenuPopup.ShowAtMouse();
        }

        #endregion

        #region Команда [OpenAttachTagPopupCommand]

        [RelayCommand]
        private void OpenAttachTagPopupCommand(UIElement? tagret)
        {
            if (PasswordMenuPopup.IsOpen)
                PasswordMenuPopup.HideCommand.Execute(null);

            AttachTagsPopup.ShowCommand.Execute(tagret);
        }

        #endregion

        // =================Tag======================

        #region Команда [CreateTagCommand]: Создает тэг

        [RelayCommand(CanExecute = nameof(CanCreateTag))]
        private async Task CreateTag()
        {
            var result = await _tagService.CreateAsync(new CreateTagRequest(_userContext.Id, NameTag!, Helpers.ColorConverter.RgbToHex(int.Parse(Red), int.Parse(Green), int.Parse(Blue))));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            Tags.Add(new TagViewModel(new TagResponse(result.Value, _userContext.Id, NameTag!, Helpers.ColorConverter.RgbToHex(int.Parse(Red), int.Parse(Green), int.Parse(Blue)))));

            NameTag = string.Empty;
        }

        private bool CanCreateTag() => !string.IsNullOrWhiteSpace(NameTag);

        #endregion

        #region Команда [DeleteTagCommand]: Удаляет тэг

        [RelayCommand]
        private async Task DeleteTag()
        {
            var result = await _tagService.DeleteAsync(SelectedTag!.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            Tags.Remove(SelectedTag);
        }

        #endregion

        #region Команда [UpdateTagCommand]: Обновляет изменение в тэге

        [RelayCommand]
        private async Task UpdateTag()
        {
            var result = await _tagService.UpdateAsync(new UpdateTagRequest(SelectedTag!.Id, SelectedTag.TagName!, SelectedTag.HexColor));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                SelectedTag.RevertChanges();
                return;
            }

            SelectedTag.CommitChanges(new TagResponse(SelectedTag.Id, _userContext.Id, SelectedTag.TagName!, SelectedTag.HexColor));
        }

        #endregion

        // ================Icon======================

        #region Команда [SaveIconCommand]: Отвечает за сохранение Svg иконки

        [RelayCommand(CanExecute = nameof(CanSaveIcon))]
        private async Task SaveIcon()
        {
            var result = await _iconService.CreatePersonalAsync(new CreateIconPersonalRequest(_userContext.Id, SvgCode!, "Тестовое название", SelectedIconCategory!.Id));

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }
            var iconVm = new IconViewModel(new IconResponse(result.Value, _userContext.Id, SvgCode!, "", SelectedIconCategory.Id));
            iconVm.SetIcon(ConvertSvgInString(iconVm.SvgCode!));
            var ict = IconCategories.FirstOrDefault(ic => ic.Id == SelectedIconCategory.Id);
            iconVm.SetCategory(ict);

            Icons.Add(iconVm);
           
            Svg = null;
            SvgCode = string.Empty;
            SelectedIconCategory = null;
        }

        private bool CanSaveIcon() => !string.IsNullOrWhiteSpace(SvgCode) && SelectedIconCategory != null;

        #endregion

        // ============IconCategory==================

        #region Команда [SaveIconCategoryCommand]: Отвечает за сохранение категории иконки

        [RelayCommand(CanExecute = nameof(CanSaveIconCategory))]
        private async Task SaveIconCategory()
        {
            var result = await _iconCategoryService.CreatePersonalAsync(new CreateIconCategoryPersonalRequest(IconCategoryName!, Guid.Parse(_userContext.Id)));

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            IconCategories.Add(new IconCategoryViewModel(new IconCategoryResponse(result.Value, _userContext.Id, IconCategoryName!)));

            IconCategoryName = string.Empty;

            UpdateIconCategoriesView();
        }

        private bool CanSaveIconCategory() => !string.IsNullOrWhiteSpace(IconCategoryName);

        #endregion

        #region Команда [UpdateIconCategoryCommand]: Отвечает за обновление категории

        [RelayCommand(CanExecute = nameof(CanUpdateIconCategorty))]
        private async Task UpdateIconCategorty(IconCategoryViewModel value)
        {
            var result = await _iconCategoryService.UpdatePersonalAsync(new UpdatePersonalIconCategoryRequest(Guid.Parse(value.Id), Guid.Parse(_userContext.Id), value.Name));

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            value.Comit();
            UpdateIconCategortyCommand.NotifyCanExecuteChanged();

            foreach (var icon in Icons)
            {
                if (icon.IconCategoryId == value.Id)
                    icon.SetCategory(value);
            }

            UpdateIconsView();
        }

        private bool CanUpdateIconCategorty()
        {
            if (SelectedEditableCategory is null)
                return false;

            return SelectedEditableCategory.HasChanges;
        }

        #endregion

        #region Команда [DeleteIconCategoryCommand] : Отвечает за удаление категории

        [RelayCommand]
        private async Task DeleteIconCategory(IconCategoryViewModel value)
        {
            var result = await _iconCategoryService.DeletePersonalAsync(_userContext.Id, value.Id);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var valueToRemove = IconCategories.FirstOrDefault(ic => ic.Id == value.Id);

            if (valueToRemove is null)
            {
                MessageBox.Show("Элемент уже удален. Если категория не исчезла - то перезапустите приложение.");
                return;
            }

            IconCategories.Remove(valueToRemove);
        }


        #endregion

        // ==============SideMenu====================

        #region Команда [SetLeftSideMenuControlCommand]: Отвечает за выбор текущего оборажаемого контрола на левой боковой понели

        [RelayCommand(CanExecute = nameof(CanSetLeftSideMenuControl))]
        private void SetLeftSideMenuControl(UserControlsName controlName) => CurrentDisplayUserControlLeftSideMenu = controlName;

        private bool CanSetLeftSideMenuControl(UserControlsName controlsName) => CurrentDisplayUserControlLeftSideMenu != controlsName;

        #endregion

        #region Команда [SetRightSideMenuActionCommand]: Отвечает за выбор текущего действия на правой боковой понели

        [RelayCommand(CanExecute = nameof(CanSetRightSideMenuAction))]
        private void SetRightSideMenuAction(ActionOnData action) => CurrentActionRightSideMenu = action;

        private bool CanSetRightSideMenuAction(ActionOnData action) => CurrentActionRightSideMenu != action;

        #endregion

        // ====================================================================================
        //                                      МЕТОДЫ                                        
        // ====================================================================================

        #region Получение данных (API)

        public async Task GetEncreptedOverview()
        {
            var result = await _vaultService.GetAllAsync(_userContext.Id);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            foreach (var encrypted in result.Value)
            {
                var encryptedVm = new CredentialsVaultViewModel(encrypted, _cryptoServices, _userContext.Dek, Tags);
                encryptedVm.Icon = ConvertSvgInString(encryptedVm.SvgCode!);

                if (encrypted.IsArchive)
                {
                    ArchivedPasswords.Add(encryptedVm);
                    continue;
                }

                if (encrypted.IsInTrash)
                {
                    TrashPasswords.Add(encryptedVm);
                    continue;
                }

                Passwords.Add(encryptedVm);
            }
        }

        public async Task GetTags()
        {
            var result = await _tagService.GetAll(_userContext.Id); 

            foreach (var item in result.Value)
            {
                Tags.Add(new TagViewModel(item));
            }
        }

        public async Task GetIcons()
        {
            var result = await _iconService.GetAll();

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            foreach (var item in result.Value)
            {
                var iconVm = new IconViewModel(item);
                iconVm.SetIcon(ConvertSvgInString(iconVm.SvgCode!));
                var ict = IconCategories.FirstOrDefault(ic => ic.Id == item.IconCategoryId);
                iconVm.SetCategory(ict);
                Icons.Add(iconVm);
            }

            UpdateIconsView();
        }

        public async Task GetIconCategories()
        {
            var result = await _iconCategoryService.GetAllAsync(_userContext.Id);

            foreach (var item in result.Value)
            {
                var iconCategoryVM = new IconCategoryViewModel(item);
                IconCategories.Add(iconCategoryVM);
            }    
        }

        #endregion

        #region Svg

        private DrawingImage? ConvertSvgInString(string svgCode)
        {
            if (string.IsNullOrWhiteSpace(svgCode)) return null;

            var settings = new WpfDrawingSettings
            {
                IncludeRuntime = true,
                TextAsGeometry = true
            };

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(svgCode));
            FileSvgReader? reader = null;

            try
            {
                reader = new FileSvgReader(settings);
                DrawingGroup drawingGroup = reader!.Read(stream);

                if (drawingGroup != null)
                {
                    var drawingImage = new DrawingImage(drawingGroup);
                    drawingImage.Freeze();
                    return drawingImage;
                }

                return null;
            }
            catch (Exception ex)
            {
                SvgConvertError = ex.Message;
            }

            return null;
        }

        private static string? ReplaceDoubleQuotesWithSingle(string inputString) => inputString?.Replace("\"", "'");

        #endregion

        #region ViewModels

        private void CreateViewModelForType(VaultType type, CredentialsVaultViewModel encryptedVm)
        {
            var encrypted = encryptedVm;

            encrypted ??= new(new EncryptedVaultResponse(string.Empty, SelectedPasswordType.Key.ToString(), DateTime.UtcNow, null, null, false, false, false, [], [], []), _cryptoServices, _userContext.Dek, Tags);

            SelectedCredentialItemBaseViewModel = type switch
            {
                VaultType.Password => new StandardPasswordViewModel(encrypted),
                VaultType.Server => new ServerPasswordViewModel(encrypted),
                VaultType.ApiKey => new ApiKeyViewModel(encrypted),
                VaultType.CreditCard => new CreditCardViewModel(encrypted),
                _ => null,
            };
        }

        private void SetReadOnly(ActionOnData action)
        {
            if (action == ActionOnData.View)
                SelectedCredentialItemBaseViewModel?.SetIsReadOnly(true);
            else
                SelectedCredentialItemBaseViewModel?.SetIsReadOnly(false);
        }

        #endregion

        #region Управление правым боковым меню

        [ObservableProperty]
        private double _rightSidebarWidth = 250;

        [ObservableProperty]
        private bool _isSidebarOpen;

        public void ToggleSidebar()
        {
            IsSidebarOpen = !IsSidebarOpen;

            if (IsSidebarOpen)
                RightSidebarWidth = 250;
            else
                RightSidebarWidth = 0;
        }

        #endregion

        #region ICollectionView 

        private void UpdateIconsView()
        {
            IconView.SortDescriptions.Clear();
            IconView.GroupDescriptions.Clear();

            IconView.SortDescriptions.Add(new SortDescription(nameof(IconViewModel.IconCategoryName), ListSortDirection.Ascending));
            IconView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(IconViewModel.IconCategoryName)));
        }

        private void UpdateIconCategoriesView()
        {
            IconCategoryView.SortDescriptions.Clear();

            IconCategoryView.SortDescriptions.Add(new SortDescription(nameof(IconCategoryViewModel.Name), ListSortDirection.Ascending));
        }

        #endregion

        #region Взоимодейсвтие с ICollectionView

        private void UpdatePasswordsView() => PasswordsView.Refresh();

        private void UpdateGroupingPassword()
        {
            PasswordsView.GroupDescriptions.Clear();
            PasswordsView.SortDescriptions.Clear();

            var sorting = ListSortDirection.Descending;

            Action action = SelectedGrouping switch
            {
                GroupingView.Name => () => 
                {
                    PasswordsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CredentialsVaultViewModel.ServiceNameFirstLetter)));
                    PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.ServiceNameFirstLetter), sorting));
                }
                ,
                GroupingView.Add => () => 
                {
                    PasswordsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CredentialsVaultViewModel.DateOnlyAdd)));
                    PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.DateOnlyAdd), sorting));
                }
                ,
                GroupingView.Update => () =>
                {
                    PasswordsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CredentialsVaultViewModel.DateOnlyUpdate)));
                    PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.DateOnlyUpdate), sorting));
                }
                ,
                GroupingView.VaultType => () =>
                {
                    PasswordsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CredentialsVaultViewModel.VaultTypeString)));
                    PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.VaultTypeString), sorting));
                }
                ,
                GroupingView.None or _ => () => PasswordsView.SortDescriptions.Add(new SortDescription(nameof(CredentialsVaultViewModel.DateAdded), sorting)),
            };

            action?.Invoke();

            UpdatePasswordsView();
        }

        #endregion

    }
}