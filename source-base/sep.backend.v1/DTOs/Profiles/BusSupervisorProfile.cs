using AutoMapper;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class BusSupervisorProfile: Profile
    {
        public BusSupervisorProfile() { 
            CreateMap<BusSupervisor, ProfileBusSupervisorDTO>();
            CreateMap<UpdateProfileBusSupervisorDTO, BusSupervisor>();
            CreateMap<CreateBusSupervisorDTO, BusSupervisor>()
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => StatusAccount.Inactive))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(DefaultPassword.BUS_SUPERVISOR_PASSWORD)))
                .ForMember(dest => dest.ShortRoleName, opt => opt.MapFrom(src => ShortRoleName.BUS_SUPER_VISOR));

            CreateMap<BusSupervisor, BusSupervisorAccountDetailDTO>()
                .ForMember(dest => dest.AccountStatusName, opt => opt.MapFrom(src => GetAccountStatusName((StatusAccount)src.AccountStatus)))
                .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => GetGender(src.Gender)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<UpdateBusSupervisorDTO, BusSupervisor>()
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.AccountStatus));
            CreateMap<BusSupervisor, BusSupervisorDTO>().ReverseMap();
        }
        private string GetAccountStatusName(StatusAccount status)
        {
            return status switch
            {
                StatusAccount.Active => "Đang Kích hoạt",
                StatusAccount.Inactive => "Chưa kích hoạt",
                StatusAccount.Deleted => "Đã vô hiệu hóa",
                _ => "Unknown"
            };
        }
        private string GetGender(bool? gender)
        {
            return gender switch
            {
                true => "Nam",
                false => "Nữ",
            };
        }

    }
}
