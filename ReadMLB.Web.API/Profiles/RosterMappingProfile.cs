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
                .ForMember(dest => dest.PlayerFirstName, opt => opt.MapFrom(src => src.Player.FirstName))
                .ForMember(dest => dest.PlayerLastName, opt => opt.MapFrom(src => src.Player.LastName));
        }
    }
}