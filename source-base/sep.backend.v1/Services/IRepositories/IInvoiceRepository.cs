using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<Invoice> GetInvoiceByIdAsync(int invoiceId);
    Task<List<Invoice>> GetAllInvoicesAsync(int? status, string? keyword);
    Task<List<Invoice>> GetInvoiceOfSchoolAsync(int schoolId, int? status = null, DateTime ? startDate = null, DateTime? endDate = null);
    Task<Invoice> GetInvoiceDetailAsync(int invoiceId);
    Task<Invoice?> GetNewestInvoiceAsync(int subscriptionId);
}