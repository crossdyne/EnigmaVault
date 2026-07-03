using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using EnigmaVault.Password.Service.Domain.Enums;
using EnigmaVault.Password.Service.Domain.ValueObjects.Password;
using EnigmaVault.Password.Service.Domain.ValueObjects.Tag;
using EnigmaVault.Password.Service.Domain.ValueObjects.User;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Primitives;

namespace EnigmaVault.Password.Service.Domain.Models
{
    public sealed class VaultItem : AggregateRoot<VaultItemId>
    {
        public UserId UserId { get; private set; }
        public VaultType PasswordType { get; private set; }

        public EncryptedData EncryptedOverview { get; private set; }
        public EncryptedData EncryptedDetails { get; private set; }

        public bool IsFavorite { get; private set; }
        public bool IsArchive { get; private set; }
        public bool IsInTrash { get; private set; }

        public DateTime? DeletedAt { get; private set; } 
        public DateTime DateAdded { get; private set; }
        public DateTime? DateUpdated { get; private set; }

        public IconId IconId { get; private set; }

        private readonly List<TagId> _tags = [];
        public IReadOnlyCollection<TagId> Tags => _tags.AsReadOnly();

        private VaultItem() { }

        private VaultItem(VaultItemId id, UserId userId, VaultType type, IconId iconId, EncryptedData encryptedOverview, EncryptedData encryptedDetails, bool isFavorite) : base(id)
        {
            UserId = userId;
            IconId = iconId;
            PasswordType = type;
            EncryptedOverview = encryptedOverview;
            EncryptedDetails = encryptedDetails;
            IsFavorite = isFavorite;
        }

        public static VaultItem Create(UserId UserId, VaultType type, IconId iconId, EncryptedData encryptedOverview, EncryptedData encryptedDetails, bool isFavorite = false)
        {
            return new VaultItem(
                VaultItemId.New(),
                UserId,
                type,
                iconId,
                encryptedOverview,
                encryptedDetails,
                isFavorite)
            {
                DateAdded = DateTime.UtcNow,
            };
        }

        public void UpdateOverview(EncryptedData encryptedOverview)
        {
            EncryptedOverview = encryptedOverview;
            UpdateDate();
        }

        public void UpdateDetails(EncryptedData encryptedDetails)
        {
            EncryptedDetails = encryptedDetails;
            UpdateDate();
        }

        public void SetFavorite(bool isFavorite)
        {
            if (IsFavorite == isFavorite) return;

            IsFavorite = isFavorite;
        }

        public void SetIcon(IconId iconId)
        {
            IconId = iconId;
        }

        public void SetArchive(bool isArchive)
        {
            if (IsArchive == isArchive)
                return;
            
            Guard.Against.That(isArchive && IsInTrash, () => new DomainException(new Error(AppErrors.Validation, "Нельзя архивировать запись, находящуюся в корзине")));

            IsArchive = isArchive;
            UpdateDate();
        }

        public void SetInTrash(bool isInTrash)
        {
            if (IsInTrash == isInTrash)
                return;

            if (isInTrash)
            {
                IsInTrash = true;
                DeletedAt = DateTime.UtcNow.AddDays(30);
                IsArchive = false; 
            }
            else
            {
                IsInTrash = false;
                DeletedAt = null;
            }
            IsInTrash = isInTrash;
            UpdateDate();
        }

        public void AddTag(TagId tagId)
        {
            if (_tags.Contains(tagId))
                return;

            _tags.Add(tagId);
        }

        public void RemoveTag(TagId tagId)
        {
            if (!_tags.Contains(tagId))
                return;

            _tags.Remove(tagId);
        }

        public void ClearTags() => _tags.Clear();

        private void UpdateDate() => DateUpdated = DateTime.UtcNow;

    }
}