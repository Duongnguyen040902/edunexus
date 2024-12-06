using AutoMapper;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterDTO, School>()
                .ForMember(dest => dest.Password,
                           opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(dest => dest.ShortRoleName,
                           opt => opt.MapFrom(src => ShortRoleName.SCHOOL_ADMIN)) 
                .ForMember(dest => dest.SchoolSubscriptionPlans,
                           opt => opt.MapFrom(src => new List<SchoolSubscriptionPlan>()));
        }
    }
}
