using AutoMapper;
using EnigmaVault.Secret.Service.Domain.Models;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Service.Application.Mappers.Profiles
{
    public sealed class FolderProfile : Profile
    {
        public FolderProfile()
        {
            CreateMap<Folder, FolderResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ParentFolderId, opt => opt.MapFrom(src => src.ParentFolderId != null ? src.ParentFolderId : (Guid?)null))
                .ForMember(dest => dest.FolderName, opt => opt.MapFrom(src => src.FolderName))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
        }
    }
}