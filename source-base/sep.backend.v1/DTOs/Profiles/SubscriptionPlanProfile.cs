using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class SubscriptionPlanProfile : Profile
    {
        public SubscriptionPlanProfile()
        {
            CreateMap<SubscriptionPlan, SubscriptionPlanDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src =>
                    src.FeatureAccesses.Select(fa => fa.Feature)));

            CreateMap<SubscriptionPlanDTO, SubscriptionPlan>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.DurationDays, opt => opt.MapFrom(src => src.DurationDays));
            
            CreateMap<SubscriptionPlan, SubscriptionPlanDetailDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.DurationDays, opt => opt.MapFrom(src => src.DurationDays))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateTime.Now.AddDays(src.DurationDays)));
        }
    }
}