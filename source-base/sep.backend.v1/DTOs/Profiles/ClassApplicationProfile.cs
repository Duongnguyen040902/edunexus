using AutoMapper;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;

namespace sep.backend.v1.DTOs.Profiles
{
    public class ClassApplicationProfile : Profile
    {
        public ClassApplicationProfile()
        {
            CreateMap<ClassApplication, ClassAppicationDTO>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ReverseMap();

            CreateMap<ClassApplication, GetClassAppicationDetailDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Pupil.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Pupil.LastName))
                .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src => src.Pupil.DonorName))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ClassApplicationCategory.Name))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => MapStatusToString(src.Status)))
                .ReverseMap();

            CreateMap<CreateAndUpdateClassApplicationDTO, ClassApplication>().ReverseMap();
            CreateMap<ResponeClassApplicationDTO, ClassApplication>().ReverseMap();
            CreateMap<ClassApplicationCategory, ClassApplicationCategoryDTO>().ReverseMap();
        }

        private string MapStatusToString(int status)
        {
            return status switch
            {
                (int)ClassApplicationStatus.Pending => "Chưa phản hồi",
                (int)ClassApplicationStatus.Approved => "Chấp thuận",
                (int)ClassApplicationStatus.Rejected => "Từ chối",
                _ => "Unknown"
            };
        }


    }
}
