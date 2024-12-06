using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class BusEnrollmentProfile : Profile
    {
        public BusEnrollmentProfile() {    
            CreateMap<BusEnrollment, BusEnrollmentDTO>()
                .ForMember(dest => dest.PupilName, opt => opt.MapFrom(src => src.Pupil != null ? (src.Pupil.FirstName + " " + src.Pupil.LastName) : null))
                .ForMember(dest => dest.PupilCode, opt => opt.MapFrom(src => src.Pupil != null ? src.Pupil.Username : null))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester != null ? src.Semester.SemesterName : null))
                .ForMember(dest => dest.AcademicYear, opt => opt.MapFrom(src => src.Semester != null ? src.Semester.SchoolYear.Name : null))
                .ForMember(dest => dest.BusName, opt => opt.MapFrom(src => src.Bus != null ? src.Bus.Name : null))
                .ForMember(dest => dest.BusSupervisorName, opt => opt.MapFrom(src => src.BusSupervisor != null ? ( src.BusSupervisor.FirstName +" " + src.BusSupervisor.LastName) : null))
                .ForMember(dest => dest.BusSupervisorCode, opt => opt.MapFrom(src => src.BusSupervisor != null ? (src.BusSupervisor.Username) : null))
                .ForMember(dest => dest.BusStopName, opt => opt.MapFrom(src => src.BusStop != null ? src.BusStop.Name : null));
            CreateMap<CreateBusEnrollmentDTO, BusEnrollment>().ReverseMap();
        }
    }
}
