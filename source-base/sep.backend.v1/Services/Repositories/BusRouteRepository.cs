using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class BusRouteRepository : Repository<BusRoute>, IBusRouteRepository
    {
        public BusRouteRepository(ApplicationContext context, ILogger<Repository<BusRoute>> logger) : base(context, logger)
        {
        }

        public async Task<List<BusRoute>> getAllBusRoute(int? status, string? keyword, int? schoolId)
        {
            var query = _context.BusRoutes.AsQueryable();

            if (status.HasValue)
                query = query.Where(br => br.Status == status.Value);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(br => br.Name.Contains(keyword) || (br.Description != null && br.Description.Contains(keyword)));

            if (schoolId.HasValue)
                query = query.Where(br => br.SchoolId == schoolId.Value);

            return await query.ToListAsync();
        }

    }
}
