using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationContext context, ILogger<Repository<Invoice>> logger) : base(context,
            logger)
        {
        }

        public Task<Invoice> GetInvoiceByIdAsync(int invoiceId)
        {
            return _context.Invoices
                .Include(i => i.Payments)
                .Include(i => i.SchoolSubscriptionPlans)
                .ThenInclude(ssp => ssp.SubscriptionPlan)
                .Include(i => i.SchoolSubscriptionPlans)
                .ThenInclude(ssp => ssp.School)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync(int? status,
            string? keyword)
        {
            var query = BuildQuery(status, keyword);

            return await query.OrderByDescending(i => i.Id).ToListAsync();
        }

        private IQueryable<Invoice> BuildQuery(int? status, string? keyword)
        {
            var query = _context.Invoices
                .Include(i => i.Payments)
                .Include(i => i.SchoolSubscriptionPlans)
                .ThenInclude(ssp => ssp.SubscriptionPlan)
                .Include(i => i.SchoolSubscriptionPlans)
                .ThenInclude(ssp => ssp.School)
                .AsQueryable();

            if (status.HasValue) query = query.Where(i => i.Status == status.Value);

            if (!string.IsNullOrEmpty(keyword))
            {
                var lowerKeyword = keyword.ToLower();
                query = query.Where(i => i.Id.ToString().ToLower().Contains(lowerKeyword)
                                         || i.IssueDate.ToString().ToLower().Contains(lowerKeyword)
                                         || i.DueDate.ToString().ToLower().Contains(lowerKeyword)
                                         || i.SchoolSubscriptionPlans.School.Name.ToLower().Contains(lowerKeyword)
                                         || i.SchoolSubscriptionPlans.SubscriptionPlan.Name.ToLower()
                                             .Contains(lowerKeyword)
                                         || i.SchoolSubscriptionPlans.SubscriptionPlan.Price.ToString().ToLower()
                                             .Contains(lowerKeyword)
                                         || i.SchoolSubscriptionPlans.SubscriptionPlan.DurationDays.ToString().ToLower()
                                             .Contains(lowerKeyword)
                );
            }

            return query;
        }
        public async Task<List<Invoice>> GetInvoiceOfSchoolAsync(
        int schoolId,
        int? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null
        )
        {
            var query = _context.Invoices
                                .Include(i => i.Payments)
                                .Include(i => i.SchoolSubscriptionPlans)
                                .ThenInclude(ssp => ssp.SubscriptionPlan)
                                .Where(i => i.SchoolSubscriptionPlans.SchoolId == schoolId);

            if (status.HasValue)
            {
                query = query.Where(i => i.Status == status.Value);
            }

            if (startDate.HasValue || endDate.HasValue)
            {
                query = query.Where(i =>
                    (!startDate.HasValue || i.IssueDate >= startDate.Value) &&
                    (!endDate.HasValue || i.IssueDate <= endDate.Value));
            }

            query = query.OrderByDescending(i => i.Id);

            return await query.ToListAsync();
        }



        public async Task<Invoice?> GetInvoiceDetailAsync(int invoiceId)
        {
            return await _context.Invoices
                                 .Include(i => i.Payments)
                                 .Include(i => i.SchoolSubscriptionPlans)
                                 .ThenInclude(i => i.SubscriptionPlan)
                                 .FirstOrDefaultAsync(i => i.Id == invoiceId);

        }

        public async Task<Invoice?> GetNewestInvoiceAsync(int subscriptionPlanId)
        {
            return await _context.Invoices
                                 .Include(i => i.SchoolSubscriptionPlans)
                                 .ThenInclude(i => i.SubscriptionPlan)
                                 .Where(i => i.SchoolSubscriptionPlans != null &&
                                             i.SchoolSubscriptionPlans.SubscriptionPlanId == subscriptionPlanId)
                                 .OrderByDescending(i => i.SchoolSubscriptionPlans.StartDate)
                                 .ThenByDescending(i => i.IssueDate)
                                 .FirstOrDefaultAsync();
        }
    }
}