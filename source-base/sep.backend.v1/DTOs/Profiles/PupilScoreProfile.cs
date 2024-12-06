using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class PupilScoreProfile : Profile
    {
        public PupilScoreProfile()
        {
            CreateMap<PupilScore, PupilScoreDTO>();
            CreateMap<PupilScoreDTO, PupilScore>();
            CreateMap<PupilScore, PupilScoreViewDTO>()
                .ForMember(dest => dest.PupilName, opt => opt.MapFrom(src => src.Pupil.FirstName + " " + src.Pupil.LastName))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Pupil.Image))
                ;
            CreateMap<PupilScoreViewDTO, PupilScore>();
        }
    } 
}
