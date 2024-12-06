using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class BusRouteProfile : Profile
    {
        public BusRouteProfile() {
            CreateMap<BusRoute, BusRouteDetailDTO>()
             .ForMember(dest => dest.BusStops, opt => opt.MapFrom(src => src.BusStops))
             .ForMember(dest => dest.Buses, opt => opt.MapFrom(src => src.Buses));

            CreateMap<BusStop, BusStopViewDTO>();

            CreateMap<Bus, BusViewDTO>();

            CreateMap<BusRoute, BusRouteDTO>().ReverseMap();
            CreateMap<CreateBusRouteDto, BusRoute>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.SchoolId, opt => opt.Ignore()); 
        }
    }
}
