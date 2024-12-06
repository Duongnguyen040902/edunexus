using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IInvoiceService : IBaseService<InvoiceDTO, Invoice>
    {
        Task<CreateInvoiceDTO> CreateInvoiceAsync(CreateInvoiceDTO model);
        Task<InvoiceDTO> GetInvoiceByIdAsync(int invoiceId);
        Task<PagedResponse<List<InvoiceDTO>>> GetAllInvoicesAsync(PaginationFilter filters, IUriService uriService,
            string route, int? status, string? keyword);
        Task<bool> UpdateInvoiceAsync(CreateInvoiceDTO model, int id);
        Task<bool> DeleteMultipleInvoiceAsync(IEnumerable<int> invoiceIds);
        Task<bool> ScheduleInvoiceAsync();
        Task<PaymentDTO> CreatePaymentAsync(PaymentDTO model);
        Task<PagedResponse<List<InvoiceDTO>>> GetAllInvoiceOfSchoolAsync
            (
            PaginationFilter filters, 
            IUriService uriService, 
            string route, 
            int schoolId, 
            int? invoiceStatus = null, 
            DateTime? startDate = null,
            DateTime? endDate = null);
        Task<InvoiceDTO> GetNewestInvoice(int subscriptionPlanId);
    }
}