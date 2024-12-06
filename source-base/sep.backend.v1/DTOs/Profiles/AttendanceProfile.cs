using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<AttendanceRecord, AttendanceRecordViewDTO>()
                    .ForMember(dest => dest.PupilName, opt => opt.MapFrom(src => src.Pupil.FirstName + " " + src.Pupil.LastName))
                    .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Pupil.Image));

            CreateMap<AttendanceRecord, AttendanceRecordDTO>();
            CreateMap<AttendanceRecordDTO, AttendanceRecord>();
        }
    }
}
