using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class BusRepository : Repository<Bus>, IBusRepository
    {
        public BusRepository(ApplicationContext context, ILogger<Repository<Bus>> logger) : base(context, logger)
        {
        }

        public async Task<Bus> GetBusDetailAsync(int busId, int semesterId)
        {
            var busEntity = await _context.Buses
                .Include(x => x.BusRoute)
                .ThenInclude(x => x.BusStops)
                .Include(x => x.BusEnrollments.Where(ce => ce.SemesterId == semesterId))
                .ThenInclude(ce => ce.Semester).ThenInclude(x => x.SchoolYear)
                .Include(c => c.BusEnrollments.Where(ce => ce.SemesterId == semesterId))
                .ThenInclude(ce => ce.BusSupervisor)
                .Include(c => c.BusEnrollments.Where(ce => ce.SemesterId == semesterId))
                .ThenInclude(ce => ce.Pupil)
                .Include(c => c.BusEnrollments.Where(ce => ce.SemesterId == semesterId))
                .ThenInclude(ce => ce.BusStop)
                .FirstOrDefaultAsync(c => c.Id == busId);
            
            return busEntity;
        }
        
         public async Task<List<Bus>> GetBuses(int? status, string? keyword, int? schoolId, int busRouteId)
        {
            var query = _context.Buses.AsQueryable();
            if (status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(b => b.Name.ToLower().Contains(keyword.ToLower()) || b.DriverName.ToLower().Contains(keyword.ToLower()));
            }
            if (schoolId.HasValue)
            {
                query = query.Where(b => b.BusRoute != null && b.BusRoute.SchoolId == schoolId.Value);
            }
            query = query.Where(b => b.BusRoute != null && b.BusRouteId == busRouteId);
           
            return await query.ToListAsync();
        }
    }
}
