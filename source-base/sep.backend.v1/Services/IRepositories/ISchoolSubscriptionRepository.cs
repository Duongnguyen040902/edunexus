using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface ISchoolSubscriptionRepository : IRepository<SchoolSubscriptionPlan>
    {
        Task<List<SchoolSubscriptionPlan>> GetAllSubscriptionOfSchoolAsync(
            int schoolId,
            int? status = null,
            int? year = null);
        Task<SchoolSubscriptionPlan?> GetCurrentSubscriptionOfSchoolAsync(int schoolId);
        Task<SchoolSubscriptionPlan?> GetSubscriptionByInvoiceIdAsync(int invoiceId);
    }
}
