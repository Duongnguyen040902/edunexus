using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class SubscriptionPlanRepository : Repository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(ApplicationContext context, ILogger<Repository<SubscriptionPlan>> logger) : base(context, logger)
        {
        }

        public async Task<List<SubscriptionPlan>> GetAllSubscriptionAsync()
        {
            return await _context.SubscriptionPlans
                                     .Include(sp => sp.FeatureAccesses)
                                     .ThenInclude(fa => fa.Feature)
                                     .ToListAsync();
        }
    }
}
