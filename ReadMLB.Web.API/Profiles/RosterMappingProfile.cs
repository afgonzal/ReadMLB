using System;
using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using ReadMLB.Entities;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Profiles
{
    public class RosterMappingProfile : Profile
    {
        public RosterMappingProfile()
        {
            CreateMap<RosterPosition, RosterModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Player.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Player.LastName))
                .ForMember(dest => dest.Shirt, opt => opt.MapFrom(src => src.Player.Shirt))
                .ForMember(dest => dest.PrimaryPosition, opt => opt.MapFrom(src => src.Player.PrimaryPosition.GetValueOrDefault().ToDescription()))
                .ForMember(dest => dest.SecondaryPosition, opt => opt.MapFrom(src =>  src.Player.SecondaryPosition.HasValue ? src.Player.SecondaryPosition.Value.ToDescription() : ""))
                .ForMember(dest => dest.Bats, opt => opt.MapFrom(src => src.Player.Bats.ToString()))
                .ForMember(dest => dest.Throws, opt => opt.MapFrom(src => src.Player.Throws.ToString()))
                ;
        }
    }
}