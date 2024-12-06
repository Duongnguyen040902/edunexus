using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class BusStopProfile : Profile
    {
        public BusStopProfile()
        {
            CreateMap<BusStop, BusStopDTO>()
                 .ForMember(dest => dest.BusRouteId, opt => opt.MapFrom(src => src.BusRouteId))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.PickUpTime, opt => opt.MapFrom(src => src.PickUpTime))
                 .ForMember(dest => dest.ReturnTime, opt => opt.MapFrom(src => src.ReturnTime))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                 .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.Index));

            CreateMap<CreateBusStopDTO, BusStop>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PickUpTime, opt => opt.MapFrom(src => src.PickUpTime))
                .ForMember(dest => dest.ReturnTime, opt => opt.MapFrom(src => src.ReturnTime))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.BusRouteId, opt => opt.MapFrom(src => src.BusRouteId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.Index));
        }
    }
}
