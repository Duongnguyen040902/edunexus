using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class FeatureProfile : Profile
    {
        public FeatureProfile() 
        {
            CreateMap<Feature, FeatureDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FeatureName, opt => opt.MapFrom(src => src.FeatureName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}
