using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class ClubEnrollmentProfile : Profile
    {
        public ClubEnrollmentProfile()
        {
            CreateMap<ClubEnrollment, ClubEnrollmentDTO>();
            CreateMap<ClubEnrollment, ClubEnrollmentDetailDTO>()
                .ForMember(dest => dest.ClubName, opt => opt.MapFrom(src => src.Club.Name))
                .ForMember(dest => dest.ClubDescription, opt => opt.MapFrom(src => src.Club.Description))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester.SemesterName))
                .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => src.Teacher));

            CreateMap<ClubEnrollmentDTO, ClubEnrollment>();
            CreateMap<AssignMemberRequest, ClubEnrollment>().ReverseMap();
            CreateMap<UpdateMemberRequest, ClubEnrollment>().ReverseMap();
            CreateMap<ClubEnrollment, ClubEnrollmentForAdminSchoolDTO>()
            .ForMember(dest => dest.PupilName, opt => opt.MapFrom(src => src.Pupil != null ? src.Pupil.FirstName + ' ' + src.Pupil.LastName : null))
            .ForMember(dest => dest.PupilUsername, opt => opt.MapFrom(src => src.Pupil != null ? src.Pupil.Username : null))
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.FirstName + ' ' + src.Teacher.LastName : null))
            .ForMember(dest => dest.TeacherUsername, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Username : null));
        }
    }
}
