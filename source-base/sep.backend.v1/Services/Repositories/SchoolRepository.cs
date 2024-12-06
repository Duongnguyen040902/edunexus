using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories;

public class SchoolRepository : Repository<School>, ISchoolRepository
{
    public SchoolRepository(ApplicationContext context, ILogger<Repository<School>> logger) : base(context, logger)
    {
    }

    public async Task<List<School>> getAllAccountSchoolAdmin(int? status, string? keyword, int? subscriptionPlanId)
    {
        var query = BuildQuery(status, keyword, subscriptionPlanId);
        return await query.OrderByDescending(s => s.Id).ToListAsync();
    }

    public Task<School> getAccountSchoolAdminBySchoolId(int schoolId)
    {
        return _context.Schools
            .Include(ssc => ssc.SchoolSubscriptionPlans)
            .ThenInclude(sc => sc.SubscriptionPlan)
            .Include(ssc => ssc.SchoolSubscriptionPlans)
            .ThenInclude(iv => iv.Invoices).ThenInclude(p => p.Payments)
            .FirstOrDefaultAsync(ssc => ssc.Id == schoolId);
    }

    public Task<List<School>> getAllSchoolExpired()
    {
        return _context.Schools
            .Include(ssc => ssc.SchoolSubscriptionPlans)
            .ThenInclude(sc => sc.SubscriptionPlan)
            .Include(ssc => ssc.SchoolSubscriptionPlans)
            .ThenInclude(iv => iv.Invoices).ThenInclude(p => p.Payments)
            .Where(ssc => ssc.SchoolSubscriptionPlans.Any(ssp => ssp.EndDate >= DateTime.UtcNow && ssp.EndDate <= DateTime.UtcNow.AddDays(6)))
            .ToListAsync();
    }

    public async Task<object> LoginAsyncByModeSchool(string email, string password)
    {
        return await _context.Schools.LoginAsync(_context, _logger, email, password);
    }

    private IQueryable<School> BuildQuery(int? status, string? keyword, int? subscriptionPlanId)
    {
        var query = _context.Schools
            .Include(ssc => ssc.SchoolSubscriptionPlans)
                .ThenInclude(sc => sc.SubscriptionPlan)
            .Include(ssc => ssc.SchoolSubscriptionPlans)
                .ThenInclude(iv => iv.Invoices).ThenInclude(p => p.Payments)
            .AsQueryable();

        if (status.HasValue) query = query.Where(s => s.AccountStatus == status.Value);

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(s => s.Name.Contains(keyword)
                                     || s.Email.Contains(keyword)
                                     || s.Address.Contains(keyword)
                                     || s.PhoneNumber.Contains(keyword)
                                     || s.Email.Contains(keyword)
            );

        if (subscriptionPlanId.HasValue && subscriptionPlanId.Value > 0)
            query = query.Where(s =>
                s.SchoolSubscriptionPlans.Any(sp => sp.SubscriptionPlanId == subscriptionPlanId.Value && sp.Status == (int)Statuses.Active));

        return query;
    }
}