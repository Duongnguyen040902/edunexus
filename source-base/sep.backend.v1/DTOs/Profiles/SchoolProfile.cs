using AutoMapper;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.DTOs.Profiles
{
    public class SchoolProfile : Profile
    {
        public SchoolProfile()
        {
            CreateMap<School, SchoolDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.WebsiteLink, opt => opt.MapFrom(src => src.WebsiteLink))
                .ForMember(dest => dest.StandardCode, opt => opt.MapFrom(src => src.StandardCode))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.DateOfEstablishment, opt => opt.MapFrom(src => src.DateOfEstablishment))
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.AccountStatus))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => GetStatusName((StatusAccount)src.AccountStatus)))
                .ForMember(dest => dest.SchoolSubscriptionPlans,
                    opt => opt.MapFrom(src => src.SchoolSubscriptionPlans.OrderByDescending(sp => sp.Id).ToList()));

            CreateMap<CreateSchoolDTO, School>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SchoolName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.ShortRoleName, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.AccountStatus))
                .ForMember(dest => dest.Password,
                    opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ReverseMap();

            CreateMap<UpdateSchoolDTO, School>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SchoolName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.WebsiteLink, opt => opt.MapFrom(src => src.WebsiteLink))
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.AccountStatus))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.StandardCode, opt => opt.MapFrom(src => src.StandardCode))
                .ForMember(dest => dest.DateOfEstablishment, opt => opt.MapFrom(src => src.DateOfEstablishment))
                // .ForMember(dest => dest.SchoolSubscriptionPlans, opt => opt.MapFrom(src =>
                //     src.SubscriptionPlanIds.Select(id => new SchoolSubscriptionPlan
                //     {
                //         SubscriptionPlanId = id
                //     }).ToList())) //TODO:QA - SubscriptionPlanIds
                .ReverseMap();
            CreateMap<School, SchoolInfoDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            CreateMap<UpdateInfoSchoolDTO, School>()
                .ForMember(dest => dest.Image, opt => opt.Condition(src => src.ImageFile != null))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }

        private string GetStatusName(StatusAccount status)
        {
            return status switch
            {
                StatusAccount.Active => "Hoạt động",
                StatusAccount.Inactive => "Ngưng hoạt động",
                StatusAccount.Deleted => "Đã xóa",
                _ => "Chưa xác định"
            };
        }
    }
}