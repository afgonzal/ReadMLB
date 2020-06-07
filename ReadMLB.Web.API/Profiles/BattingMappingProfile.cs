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
                .ForMember(dest => dest.Organization,
                    opt => opt.MapFrom(src =>
                        src.Team.Organization != null
                            ? src.Team.Organization.TeamAbr
                            : src.Team.TeamAbr))
                
                .ForMember(dest => dest.TeamName,
                    opt => opt.MapFrom(src => src.Team.TeamName))
                .ForMember(dest => dest.TeamAbr,
                    opt => opt.MapFrom(src => src.Team.TeamAbr));
            CreateMap<Batting, BattingAndPlayerStatModel>()
                .ForMember(dest => dest.TeamAbr,
                    opt => opt.MapFrom(src => src.Team.TeamAbr))
                .ForMember(dest => dest.Organization,
                    opt => opt.MapFrom(src =>
                        src.Team.Organization != null
                            ? src.Team.Organization.TeamAbr
                            : src.Team.TeamAbr))
                .ForMember(dest => dest.OrganizationId,
                    opt => opt.MapFrom(src =>
                        src.Team.Organization != null
                            ? src.Team.Organization.OrganizationId
                            : src.Team.TeamId))
                .ForMember(dest => dest.TeamName,
                    opt => opt.MapFrom(src =>
                        src.Team.TeamName))
                .ForMember(dest => dest.PlayerName,
                    opt => opt.MapFrom(src => $"{src.Player.LastName}, {src.Player.FirstName}"));
                
        }
    }
}
