using AutoMapper;
using EnigmaVault.Password.Service.Domain.Models;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.Password.Service.Application.Mappers.Profiles
{
    internal sealed class VaultItemProfile : Profile
    {
        public VaultItemProfile()
        {
            CreateMap<VaultItem, EncryptedVaultResponse>()
             .ConstructUsing(src => new EncryptedVaultResponse(
                 src.Id.ToString(),
                 src.VaultType.ToString(),                                    
                 src.DateAdded,
                 src.DateUpdated,                                
                 src.DeletedAt,
                 src.IsFavorite,
                 src.IsArchive,
                 src.IsInTrash,
                 src.EncryptedOverview, 
                 src.EncryptedDetails,    
                 new List<string>(src.Tags.Select(x => x.Value.ToString())),
                 src.IconId.ToString()
             ));
        }
    }
}