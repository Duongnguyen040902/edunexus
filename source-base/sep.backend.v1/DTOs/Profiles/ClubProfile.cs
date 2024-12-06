using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<Data.Entities.Club, ClubDTO>();
            CreateMap<Data.Entities.Club, ClubDetailDTO>()
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School.Name))
                .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => src.ClubEnrollments.OrderByDescending(x => x.Id).FirstOrDefault(ce => ce.TeacherId != null).Teacher))
                .ForMember(dest => dest.Pupils, opt => opt.MapFrom(src => src.ClubEnrollments.Where(x => x.PupilId != null).Select(x => x.Pupil).ToList()))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.ClubEnrollments.OrderByDescending(x => x.Id).FirstOrDefault(ce => ce.SemesterId != null).Semester.SemesterName))
                .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src => src.ClubEnrollments.OrderByDescending(x => x.Id).FirstOrDefault(ce => ce.SemesterId != null).SemesterId))
                .ForMember(dest => dest.SchoolYearId, opt => opt.MapFrom(src => src.ClubEnrollments.OrderByDescending(x => x.Id).FirstOrDefault(ce => ce.SemesterId != null).Semester.SchoolYear.SchoolId))
                .ForMember(dest => dest.SchoolYearName, opt => opt.MapFrom(src => src.ClubEnrollments.OrderByDescending(x => x.Id).FirstOrDefault(ce => ce.SemesterId != null).Semester.SchoolYear.Name));
            CreateMap<CreateClubDTO, Club>();

        }
    }
}
