using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class TimeSlotRepository : Repository<TimeSlot>, ITimeSlotRepository
    {
        public TimeSlotRepository(ApplicationContext context, ILogger<Repository<TimeSlot>> logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<TimeSlot>> GetAllTimeSlots(bool? isActive, string? keyword, int schoolId)
        {
            var query = _context.TimeSlots.Where(x => x.SchoolId == schoolId).AsQueryable();
            
            if (isActive.HasValue)
                query = query.Where(ts => ts.IsActive == isActive.Value);

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(ts => ts.Name.ToLower().Contains(keyword.ToLower()));

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalRecords(bool? isActive, string? keyword)
        {
            var query = _context.TimeSlots.AsQueryable();

            if (isActive.HasValue)
                query = query.Where(ts => ts.IsActive == isActive.Value);

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(ts => ts.Name.ToLower().Contains(keyword.ToLower()));

            return await query.CountAsync();
        }
    }
}
