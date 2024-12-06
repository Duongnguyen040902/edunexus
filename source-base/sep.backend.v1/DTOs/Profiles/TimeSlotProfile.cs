using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class TimeSlotProfile : Profile
    {
        public TimeSlotProfile()
        {
            CreateMap<TimeSlot, TimeSlotDTO>();
            CreateMap<TimeSlotDTO, TimeSlot>();
            CreateMap<CreateTimeSlotDTO, TimeSlot>().ReverseMap();
        }
    }
}
