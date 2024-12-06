using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class BusStopRepository : Repository<BusStop>, IBusStopRepository
    {
        public BusStopRepository(ApplicationContext context, ILogger<Repository<BusStop>> logger) : base(context, logger)
        {
        }

        public async Task<List<BusStop>> GetBusStops(int? status, string? keyword, int busRouteId)
        {
            var query = _context.Set<BusStop>().AsQueryable();

            query = query.Where(x => x.BusRouteId == busRouteId);

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Address.Contains(keyword));
            }
            query = query.OrderBy(x => x.Index);

            return await query.ToListAsync();
        }
    }
}
