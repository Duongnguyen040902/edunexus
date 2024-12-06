using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class NotificationRepository : Repository<Notifications>, INotificationRepository
    {
        public NotificationRepository(ApplicationContext context, ILogger<Repository<Notifications>> logger) : base(context, logger)
        {
        }

        public async Task<List<Notifications>> GetNotifications(int Classid)
        {
            return await _context.Notifications.Where(x => x.ClassId == Classid).Include(c => c.Category).ToListAsync();
        }

        public async Task<Notifications> GetNotificationsAsync(int id)
        {
            return await _context.Notifications.Include(c => c.Category).Include(i => i.Images).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
