using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class BusProfile : Profile
    {
        public BusProfile()
        {
            CreateMap<Data.Entities.Bus, BusDTO>();
            CreateMap<Data.Entities.Bus, ViewBusDetailDTO>()
                .ForMember(dest => dest.BusRouteName, opt => opt.MapFrom(src => src.BusRoute.Name))
                .ForMember(dest => dest.BusSupervisor, opt => opt.MapFrom(src => src.BusEnrollments.OrderByDescending(x => x.Id).FirstOrDefault(ce => ce.BusSupervisorId != null).BusSupervisor))
                .ForMember(dest => dest.Pupil, opt => opt.MapFrom(src => src.BusEnrollments.Where(x => x.PupilId != null && x.BusStopId != null).Select(x => new PupilOnBusDTO
                {
                    Id = x.Id,
                    PupilId = x.Id,
                    PupilName = x.Pupil.FirstName + " " + x.Pupil.LastName,
                    Image = x.Pupil.Image,
                    DonorName = x.Pupil.DonorName,
                    DonorPhoneNumber = x.Pupil.DonorPhoneNumber,
                    Address = x.Pupil.Address,
                    DateOfBirth = x.Pupil.DateOfBirth,
                    Gender = x.Pupil.Gender,
                    BusStopId = x.BusStopId,
                    BusStopAddress = x.BusStop.Address,
                }).ToList()))
                .ForMember(dest => dest.BusStops, opt => opt.MapFrom(src => src.BusRoute.BusStops.Select(bs => new ViewBusStopDTO
                {
                    Id = bs.Id,
                    Name = bs.Name,
                    EstimatedTime = bs.PickUpTime,
                    Address = bs.Address,
                    BusRouteId = bs.BusRouteId,
                    Status = bs.Status
                }).ToList()));
            CreateMap<Data.Entities.BusSupervisor, BusSupervisorDetailDTO>();
            CreateMap<Data.Entities.BusStop, ViewBusStopDTO>();
            CreateMap<Bus, BusDetailDto>().ReverseMap();
            CreateMap<CreateBusDto, Bus>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap(); 
            CreateMap<Data.Entities.BusEnrollment, ViewBusEnrollDetailDTO>()
                .ForMember(dest => dest.BusName, opt => opt.MapFrom(src => src.Bus.Name))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester.SemesterName))
                .ForMember(dest => dest.SchoolYearId, opt => opt.MapFrom(src => src.Semester.SchoolYearId))
                .ForMember(dest => dest.SchoolYearName, opt => opt.MapFrom(src => src.Semester.SchoolYear.Name))
                .ForMember(dest => dest.IsCurrent, opt => opt.MapFrom(src => src.Semester.IsActive))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Semester.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Semester.EndDate))
                .ForMember(dest => dest.BusStopId, opt => opt.MapFrom(src => src.BusStopId))
                .ForMember(dest => dest.BusStopName, opt => opt.MapFrom(src => src.BusStop.Name))
                .ForMember(dest => dest.PickUpTime, opt => opt.MapFrom(src => src.BusStop.PickUpTime))
                .ForMember(dest => dest.ReturnTime, opt => opt.MapFrom(src => src.BusStop.ReturnTime))
               ;
            
        }
    }
}
