using AutoMapper;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class ClassProfile : Profile
    {

        public ClassProfile()
        {

            CreateMap<Class, ClassDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Name))
                //.ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School != null ? src.School.Name : string.Empty))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.SchoolId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));


            CreateMap<Class, ClassDetailDTO>()
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School != null ? src.School.Name : string.Empty))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.SchoolId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.HomeroomTeacher, opt => opt.MapFrom(src => src.ClassEnrollments
                    .OrderByDescending(ce => ce.Semester.EndDate)
                    .FirstOrDefault(ce => ce.TeacherId != null).Teacher))
                .ForMember(dest => dest.Pupils, opt => opt.MapFrom(src => src.ClassEnrollments
                    .Where(ce => ce.PupilId != null)
                    .Select(ce => ce.Pupil).ToList()))
                .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src => src.ClassEnrollments
                    .OrderByDescending(ce => ce.Id)
                    .FirstOrDefault().SemesterId))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.ClassEnrollments
                    .OrderByDescending(ce => ce.Id)
                    .FirstOrDefault().Semester.SemesterName))
                .ForMember(dest => dest.SchoolYearId, opt => opt.MapFrom(src => src.ClassEnrollments
                    .OrderByDescending(ce => ce.Id)
                    .FirstOrDefault().Semester.SchoolYear.SchoolId))
                .ForMember(dest => dest.SchoolYearName, opt => opt.MapFrom(src => src.ClassEnrollments
                    .OrderByDescending(ce => ce.Id)
                    .FirstOrDefault().Semester.SchoolYear.Name));

            CreateMap<Class, UpdateClassDTO>()
                       .ReverseMap();

            CreateMap<Class, ViewClassAdminDTO>().
                ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();



            CreateMap<Class, AddClassDTO>()
                   .ReverseMap();
            CreateMap<Class, ViewClassDetailDTO>()
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School != null ? src.School.Name : string.Empty))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.SchoolId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.HomeroomTeacher, opt => opt.MapFrom(src => src.ClassEnrollments
                .FirstOrDefault(ce => ce.TeacherId != null).Teacher))
                .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src => src.ClassEnrollments
                .FirstOrDefault().SemesterId))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.ClassEnrollments
                .FirstOrDefault().Semester.SemesterName))
                .ForMember(dest => dest.SchoolYearId, opt => opt.MapFrom(src => src.ClassEnrollments
                .FirstOrDefault().Semester.SchoolYear.SchoolId))
                .ForMember(dest => dest.SchoolYearName, opt => opt.MapFrom(src => src.ClassEnrollments
                .FirstOrDefault().Semester.SchoolYear.Name));

            CreateMap<ClassEnrollment, ViewClassEnrollDTO>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src =>src.Class.Name))
                .ForMember(dest => dest.SemesterName,opt => opt.MapFrom(src =>src.Semester.SemesterName))
                .ForMember(dest => dest.SchoolYearId,opt => opt.MapFrom(src =>src.Semester.SchoolYearId))
                .ForMember(dest => dest.SchoolYearName,opt => opt.MapFrom(src =>src.Semester.SchoolYear.Name))
                .ForMember(dest => dest.IsCurrent,opt => opt.MapFrom(src =>src.Semester.IsActive))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Semester.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Semester.EndDate))
                ;


        }

    }
}
