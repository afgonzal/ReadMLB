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
                .ForMember(dest => dest.TeamAbr, opt => opt.MapFrom(src => src.TeamAbr.TrimEnd()))
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(src => src.OrganizationId ?? src.TeamId))
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => src.Organization != null ? src.Organization.TeamAbr : src.TeamAbr));

            CreateMap<Division, DivisionModel>();
        }
    }
}