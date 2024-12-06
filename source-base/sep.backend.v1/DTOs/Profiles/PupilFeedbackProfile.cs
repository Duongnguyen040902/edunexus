using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class PupilFeedbackProfile : Profile
    {
        public PupilFeedbackProfile() { 
        CreateMap<PupilFeedback, PupilFeedbackDTO>();
        CreateMap<PupilFeedbackDTO, PupilFeedback>();
        CreateMap<PupilFeedback, ListPupilFeedbackDTO>()
            .ForMember(dest => dest.PupilName, opt => opt.MapFrom(src => src.Pupil.FirstName + "" + src.Pupil.LastName))
            .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src => src.Pupil.DonorName))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Pupil.Image));
        CreateMap<ListPupilFeedbackDTO, PupilFeedback>(); 
        CreateMap<PupilFeedback,PupilFeedbackDetailDTO>()
             .ForMember(dest => dest.Semester, opt=> opt.MapFrom(src => src.Semester));
        }   
    }
}
