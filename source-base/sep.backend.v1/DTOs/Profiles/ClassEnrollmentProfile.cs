using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class ClassEnrollmentProfile : Profile
    {
        public ClassEnrollmentProfile() {
            CreateMap<ClassEnrollment, AssignTeacherRequest>()
                   .ReverseMap();

            CreateMap<ClassEnrollment, AssignPupilRequest>()
                 .ReverseMap();

            CreateMap<ClassEnrollment, AssignMemberToClass>()
                 .ReverseMap();

            CreateMap<ClassEnrollment, TeacherSwapDTO>()
                 .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src =>
                     src.Teacher.FirstName + " " + src.Teacher.LastName))
                 .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Teacher.Image))
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Teacher.Username))
                 .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src => src.SemesterId))
                 .ForMember(dest => dest.ClassEnrollmentId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name));

            CreateMap<ClassEnrollment, MemberInClassDTO>()
                .ForMember(dest => dest.ClassName,
                    opt => opt.MapFrom(src => src.Class != null ? src.Class.Name : string.Empty))
                .ForMember(dest => dest.TeacherName,
                    opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.FirstName + " " + src.Teacher.LastName : string.Empty)) 
                .ForMember(dest => dest.TeacherCode,
                    opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Username : string.Empty))
                .ForMember(dest => dest.TeacherImage,
                    opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Image : string.Empty))
                .ForMember(dest => dest.PupilImage,
                    opt => opt.MapFrom(src => src.Pupil != null ? src.Pupil.Image : string.Empty))
                .ForMember(dest => dest.PupilName,
                    opt => opt.MapFrom(src => src.Pupil != null ? src.Pupil.FirstName + " " + src.Pupil.LastName : string.Empty))
                .ForMember(dest => dest.PupilCode,
                    opt => opt.MapFrom(src => src.Pupil != null ? src.Pupil.Username : string.Empty))
                .ForMember(dest => dest.Block,
                    opt => opt.MapFrom(src => src.Class != null ? src.Class.Block : 0))
                .ReverseMap();


        }
    }
}
