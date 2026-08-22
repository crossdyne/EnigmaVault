using AutoMapper;
using EnigmaVault.Password.Service.Domain.Models;
using Shared.Contracts.PasswordService.Responses;

namespace EnigmaVault.Password.Service.Application.Mappers.Profiles
{
    internal sealed class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
        }
    }
}