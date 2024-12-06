using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class SchoolSubscriptionPlanProfile : Profile
    {
        public SchoolSubscriptionPlanProfile() {
            CreateMap<SchoolSubscriptionPlan, SchoolSubscriptionPlanDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubscriptionPlan, opt => opt.MapFrom(src => src.SubscriptionPlan))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Invoices, opt => opt.MapFrom(src => src.Invoices));
        }
    }
}
