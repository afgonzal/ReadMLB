using System;
using AutoMapper;
using ReadMLB.Entities;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Profiles
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<Player, PlayerModel>()
                .ForMember(dest => dest.PrimaryPosition,
                    opt => opt.MapFrom(src => src.PrimaryPosition.GetValueOrDefault().ToDescription()))
                .ForMember(dest => dest.SecondaryPosition,
                    opt => opt.MapFrom(src =>
                        src.SecondaryPosition.HasValue ? src.SecondaryPosition.Value.ToDescription() : ""))
                .ForMember(dest => dest.Bats, opt => opt.MapFrom(src => src.Bats.ToString()))
                .ForMember(dest => dest.Throws, opt => opt.MapFrom(src => src.Throws.ToString()));
        }
    }
}
