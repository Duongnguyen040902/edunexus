using AutoMapper;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class PupilProfile : Profile
    {
        public PupilProfile()
        {
            CreateMap<Pupil, PupilDTO>();

            CreateMap<Pupil, PupilDetailDTO>()
                .ForMember(dest => dest.AccountStatusName, opt => opt.MapFrom(src => GetAccountStatusName((StatusAccount)src.AccountStatus)))
                .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => GetGender(src.Gender)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<CreatePupilDTO, Pupil>()
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => StatusAccount.Inactive))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(DefaultPassword.DONOR_PASSWORD)))
                .ForMember(dest => dest.ShortRoleName, opt => opt.MapFrom(src => ShortRoleName.DONNOR));

            CreateMap<Pupil, ProfilePupilDTO>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.PupilClasses.Where(ts => ts.PupilId == src.Id).Select(x => x.Class.Name).FirstOrDefault()));
            CreateMap<Pupil, UpdateProfilePupilDTO>();
            CreateMap<UpdateProfilePupilDTO, Pupil>();
            CreateMap<Pupil, AddPupilToClassDTO>()
                   .ReverseMap();
            CreateMap<Pupil, AddPupilToClassDTO>()
                   .ReverseMap();
            CreateMap<Pupil, ViewAdminPupilDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));
            CreateMap<UpdatePupilDTO, Pupil>()
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.AccountStatus));
             CreateMap<Pupil, PupilAssignToClubDTO>();
            //AllowNullDestinationValues = false;
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

