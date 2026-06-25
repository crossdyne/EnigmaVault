using AutoMapper;
using EnigmaVault.PasswordService.Domain.Models;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Mappers.Profiles
{
    internal sealed class VaultItemProfile : Profile
    {
        public VaultItemProfile()
        {
            CreateMap<VaultItem, EncryptedVaultResponse>()
             .ConstructUsing(src => new EncryptedVaultResponse(
                 src.Id.ToString(),
                 src.PasswordType.ToString(),                                       // Map to Type
                 src.DateAdded,
                 src.DateUpdated,                                        // Map to DateUpdate
                 src.DeletedAt,
                 src.IsFavorite,
                 src.IsArchive,
                 src.IsInTrash,
                 (byte[])src.EncryptedOverview,  // Конвертация в Base64
                 (byte[])src.EncryptedDetails,    // Конвертация в Base64
                 new List<string>(src.Tags.Select(x => x.Value.ToString())),
                 src.IconId.ToString()
             ));
        }
    }
}