using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class SemesterProfile : Profile
    {
        public SemesterProfile()
        {

            CreateMap<Semester, ViewSemesterDTO>()
                .ForMember(dest => dest.SchoolYearName, opt => opt.MapFrom(src => src.SchoolYear.Name));

            CreateMap<Semester, SemesterDTO>();
            CreateMap<SemesterDTO, Semester>();
            CreateMap<CreateAndUpdateSemesterDTO, Semester>();
        }
    }
}
