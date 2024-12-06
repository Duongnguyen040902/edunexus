using AutoMapper;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class SchoolSubscriptionProfile : Profile
    {
        public SchoolSubscriptionProfile()
        {
            // Mapping to SchoolSubscriptionPlan from SchoolSubscriptionDTO
            CreateMap<SchoolSubscriptionPlan, SchoolSubscriptionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.SchoolId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => GetSchoolSubscriptionStatusName((Statuses)src.Status)))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.SubscriptionPlanName, opt => opt.MapFrom(src => src.SubscriptionPlan.Name))
                .ForMember(dest => dest.SubscriptionPlanId, opt => opt.MapFrom(src => src.SubscriptionPlan.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SubscriptionPlan.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.SubscriptionPlan.Price))
                .ForMember(dest => dest.DurationDays, opt => opt.MapFrom(src => src.SubscriptionPlan.DurationDays))

                 // Map the list of feature names from FeatureAccess
                 .ForMember(dest => dest.Invoices, opt => opt.MapFrom(src =>
                    src.Invoices));
        }
        private string GetSchoolSubscriptionStatusName(Statuses status)
        {
            return status switch
            {
                Statuses.Active => "Đang kích hoạt",
                Statuses.Inactive => "Không kích hoạt",
                _ => "Unknown"
            };
        }
    }
}
