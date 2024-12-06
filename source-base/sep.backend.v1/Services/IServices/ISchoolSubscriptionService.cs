using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ISchoolSubscriptionService
    {
        Task<PagedResponse<List<SchoolSubscriptionDTO>>> GetAllSubscriptionsOfSchoolAsync(
            PaginationFilter filters,
            IUriService uriService,
            string route,
            int schoolId,
            int? status = null,
            int? year = null);
        Task<SchoolSubscriptionDTO?> GetCurrentSubscriptionOfSchoolAsync(int schoolId);
        Task<InvoiceDTO> CreateInvoiceForSubscriptionAsync(int schoolId, int subscriptionPlanId);
        Task<string> GeneratePaymentUrlAsync(int invoiceId);
        Task<bool> ProcessPaymentCallbackAsync(HttpContext httpContext);
    }
}
