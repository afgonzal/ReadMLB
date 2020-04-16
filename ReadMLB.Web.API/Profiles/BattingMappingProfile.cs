using AutoMapper;
using ReadMLB.Entities;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Profiles
{
    public class BattingMappingProfile : Profile
    {
        public BattingMappingProfile()
        {
            CreateMap<Batting, BattingStatModel>()
                .ForMember(dest => dest.PlayerName, opt => opt.MapFrom(src => $"{src.Player.LastName}, {src.Player.FirstName}"))
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Organization != null ? $"{src.Team.TeamName} - {src.Team.Organization.TeamName}"  : $"{src.Team.TeamName}"));
        }
    }
}
