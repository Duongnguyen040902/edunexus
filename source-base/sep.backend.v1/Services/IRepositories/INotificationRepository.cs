using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface INotificationRepository : IRepository<Notifications>
    {
        Task<List<Notifications>> GetNotifications(int Classid);
        Task<Notifications> GetNotificationsAsync(int id);

    }
}
