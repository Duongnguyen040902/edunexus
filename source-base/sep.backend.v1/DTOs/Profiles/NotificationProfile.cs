using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notifications, NotificationDTO>();
            CreateMap<Notifications, DetailNotificationDTO>()
                .ForMember(dest => dest.notificationImages, opt => opt.MapFrom(src => src.Images.Where(x => x.NotificationId == src.Id)
                .ToList()));
            CreateMap<NotificationImage, NotificationImageDTO>();

            CreateMap<NotificationImageDTO, NotificationImage>();

            CreateMap<AddNotificationDTO, Notifications>();

            CreateMap<UpdateNotificationDTO, Notifications>();
        }
    }
}
