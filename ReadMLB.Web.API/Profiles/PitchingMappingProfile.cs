using AutoMapper;
using ReadMLB.Entities;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Profiles
{
    public class PitchingMappingProfile : Profile
    {
        public PitchingMappingProfile()
        {
            CreateMap<Pitching, PitchingStatModel>()
                .ForMember(dest => dest.Organization,
                    opt => opt.MapFrom(src =>
                        src.Team.Organization != null
                            ? src.Team.Organization.TeamAbr
                            : src.Team.TeamAbr))
                .ForMember(dest => dest.TeamName,
                    opt => opt.MapFrom(src => src.Team.TeamName))
                .ForMember(dest => dest.TeamAbr,
                    opt => opt.MapFrom(src => src.Team.TeamAbr));
            CreateMap<Pitching, PitchingAndPlayerStatModel>()
                .ForMember(dest => dest.PlayerName,
                    opt => opt.MapFrom(src => $"{src.Player.LastName}, {src.Player.FirstName}"))
                .ForMember(dest => dest.Organization,
                    opt => opt.MapFrom(src =>
                        src.Team.Organization != null
                            ? src.Team.Organization.TeamAbr
                            : src.Team.TeamAbr))
                .ForMember(dest => dest.TeamName,
                    opt => opt.MapFrom(src => src.Team.TeamName))
                .ForMember(dest => dest.TeamAbr,
                    opt => opt.MapFrom(src => src.Team.TeamAbr));
        }
    }
}
