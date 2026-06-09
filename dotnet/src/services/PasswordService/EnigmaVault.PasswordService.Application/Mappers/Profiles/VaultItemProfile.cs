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
                 new List<string>(src.Tags.Select(x => x.Value.ToString()))
             ));
            //CreateMap<VaultItem, EncryptedVaultOverviewResponse>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.PasswordType))
            //    .ForMember(des => des.DateAdded, opt => opt.MapFrom(src => src.DateAdded))
            //    .ForMember(des => des.DateUpdate, opt => opt.MapFrom(src => src.DateUpdated))
            //    .ForMember(des => des.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
            //    .ForMember(des => des.IsArchive, opt => opt.MapFrom(src => src.IsArchive))
            //    .ForMember(des => des.IsFavorite, opt => opt.MapFrom(src => src.IsFavorite))
            //    .ForMember(des => des.IsInTrash, opt => opt.MapFrom(src => src.IsInTrash))
            //    .ForMember(des => des.EncryptedOverview, opt => opt.MapFrom(src => src.EncryptedOverview))
            //    .ForMember(des => des.EncryptedDetails, opt => opt.MapFrom(src => src.EncryptedDetails));
        }
    }
}