using EnigmaVault.Password.Service.Domain.ValueObjects.Password;
using EnigmaVault.Password.Service.Domain.ValueObjects.Tag;

namespace EnigmaVault.Password.Service.Domain.Models
{
    public sealed class VaultTags 
    {
        public VaultItemId VaultItemId { get; private set; }
        public TagId TagId { get; private  set; }

        private VaultTags()
        {
            
        }

        private VaultTags(VaultItemId vaultItemId, TagId tagId)
        {
            VaultItemId = vaultItemId;
            TagId = tagId;
        }

        public static VaultTags Create(VaultItemId vaultItemId, TagId tagId)
        {
            return new VaultTags(vaultItemId, tagId);
        }
    }
}