using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface INotificationService:IBaseService<NotificationDTO,Notifications>
    {
        Task<List<NotificationDTO>> GetNotifications(int classid, int schoolYearId);
        Task<DetailNotificationDTO> GetNotificationAsync(int id);
        Task<bool> AddNotificationAsync(AddNotificationDTO addNotificationDTO, int schoolYearId);
        Task<bool> UpdateNotificationAsync(UpdateNotificationDTO updateNotificationDTO);
        Task<bool> DeleteNotificationAsync(int id);
        Task<List<NotificationCategory>> notificationCategories();
        Task<List<Pupil>> GetListPupil(int classId, int schoolYearId);

    }
}
