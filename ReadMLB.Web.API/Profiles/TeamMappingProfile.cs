using AutoMapper;
using ReadMLB.Entities;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Profiles
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamModel>()
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => src.Organization != null ? src.Organization.TeamAbr : src.TeamAbr));
        }
    }
}