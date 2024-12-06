using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class SchoolSubscriptionRepository : Repository<SchoolSubscriptionPlan>, ISchoolSubscriptionRepository
    {
        public SchoolSubscriptionRepository(ApplicationContext context, ILogger<Repository<SchoolSubscriptionPlan>> logger) : base(context, logger)
        {
        }

        public async Task<List<SchoolSubscriptionPlan>> GetAllSubscriptionOfSchoolAsync(
        int schoolId,
        int? status = null,
        int? year = null)
        {
            var query = _context.SchoolSubscriptionPlans
                .Include(ssp => ssp.SubscriptionPlan)
                .Include(ssp => ssp.Invoices)
                    .ThenInclude(iv => iv.Payments)
                .Where(ssp => ssp.SchoolId == schoolId);

            if (status != null)
            {
                query = query.Where(ssp => ssp.Status == status);
            }

            if (year != null)
            {
                query = query.Where(ssp => ssp.StartDate.Year == year);
            }

            query = query.Where(ssp =>
                ssp.Status == (int)Statuses.Active ||
                ssp.Invoices.Any(invoice =>
                    invoice.Payments.Any(payment =>
                        payment.Status == (int)PaymentStatuses.Success)) ||
                ssp.SubscriptionPlan.Price == 0
            );

            return await query
                .OrderByDescending(ssp => ssp.StartDate)
                .ToListAsync();
        }




        public async Task<SchoolSubscriptionPlan?> GetCurrentSubscriptionOfSchoolAsync(int schoolId)
        {
            return await _context.SchoolSubscriptionPlans
                                 .Include(ssp => ssp.SubscriptionPlan)
                                 .ThenInclude(sp => sp.FeatureAccesses)
                                 .ThenInclude(fa => fa.Feature)
                                 .Where(ssp => ssp.SchoolId == schoolId
                                               && ssp.StartDate <= DateTime.Now
                                               && ssp.EndDate >= DateTime.Now
                                               && ssp.Status == (int)Statuses.Active)
                                 .FirstOrDefaultAsync();
        }

        public async Task<SchoolSubscriptionPlan?> GetSubscriptionByInvoiceIdAsync(int invoiceId)
        {
            return await _context.SchoolSubscriptionPlans
                                 .Include(ssp => ssp.SubscriptionPlan)
                                 .Include(ssp => ssp.Invoices)
                                 .Include(ssp => ssp.School)
                                 .FirstOrDefaultAsync(ssp => ssp.Invoices.Any(invoice => invoice.Id == invoiceId));
        }

    }
}
