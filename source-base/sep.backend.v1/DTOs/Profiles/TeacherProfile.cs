using AutoMapper;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
            CreateMap<Teacher, TeacherDetailDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.AccountStatusName, opt => opt.MapFrom(src => GetAccountStatusName((StatusAccount)src.AccountStatus)))
                .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => GetGender(src.Gender)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.SubjectIds, opt => opt.MapFrom(src => src.TeacherSubjects.Select(tc => tc.Subject.Id)))
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.TeacherSubjects.Select(tc => tc.Subject.Name)));



            // Mapping from CreateTeacherDTO to Teacher
            CreateMap<CreateTeacherDTO, Teacher>()
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => StatusAccount.Inactive))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(DefaultPassword.TEACHER_PASSWORD)))
                .ForMember(dest => dest.ShortRoleName, opt => opt.MapFrom(src => ShortRoleName.TEACHER));


            CreateMap<UpdateTeacherDTO, Teacher>()  
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.AccountStatus));

            CreateMap<TeacherAccountDTO, Teacher>().ReverseMap();
             CreateMap<Teacher, AssignTeacherToClassDTO>()
                   .ReverseMap();
                     CreateMap<Teacher, AssignTeacherToClassDTO>()
               .ForMember(dest => dest.Name,
                          opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
               .ReverseMap();            
            CreateMap<Teacher, UpdateProfileTeacherDTO>();
            CreateMap<Teacher, ProfileTeacherDTO>()
            .ForMember(dest => dest.ListSubject, opt => opt.MapFrom(src => src.TeacherSubjects.Select(ts => new SubjectDTO
            {
                Id = ts.Subject.Id,
                Name = ts.Subject.Name,
                Code = ts.Subject.Code
            }).ToList()));
            CreateMap<Teacher, TeacherAssignClubDTO>()
                  .ForMember(dest => dest.Name,
                             opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                  .ReverseMap();

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
