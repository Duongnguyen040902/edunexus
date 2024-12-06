using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class TimeTableProfile : Profile
    {
        public TimeTableProfile()
        {
            CreateMap<TimeTable, TimeTableDTO>();
            CreateMap<TimeTableDTO, TimeTable>();
            CreateMap<TimeTable, TimeTableDetailDTO>()
                   .ForMember(dest => dest.ClassName,
                   opt => opt.MapFrom(src => src.Class.Name))
                   .ForMember(dest => dest.SemesterName,
                   opt => opt.MapFrom(src => src.Semester.SemesterName))
                   .ForMember(dest => dest.TimeSlotName,
                   opt => opt.MapFrom(src => src.TimeSlot.Name));
            CreateMap<CreateTimeTableDTO, TimeTable>();
            CreateMap<Semester, SemesterDTO>();

        }
    }
}
