using AutoMapper;
using ReadMLB.Entities;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Profiles
{
    public class RunningMappingProfile : Profile
    {
        public RunningMappingProfile()
        {
            CreateMap<Running, RunningStatModel>()
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
