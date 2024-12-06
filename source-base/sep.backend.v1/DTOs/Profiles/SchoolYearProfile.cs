using sep.backend.v1.Data.Entities;
using AutoMapper;
namespace sep.backend.v1.DTOs.Profiles
{
    public class SchoolYearProfile : Profile
    {
        public SchoolYearProfile()
        {
            CreateMap<SchoolYear, SchoolYearDTO>();
            CreateMap<CreateAndUpdateSchoolYearDTO, SchoolYear>();
        }
    }
}
