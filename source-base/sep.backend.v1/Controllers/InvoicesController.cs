using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : BaseApiController<InvoicesController>
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IUriService _uriService;

        public InvoicesController(ILogger<InvoicesController> logger, IInvoiceService invoiceService,
            IUriService uriService) : base(logger)
        {
            _invoiceService = invoiceService;
            _uriService = uriService;
        }

        // [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA, SPA")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetListInvoicesAsync([FromQuery] PaginationFilter filter,
            int? invoiceStatus, DateTime? startDate, DateTime? endDate, int? school)
        {
            var route = Request.Path.Value;
            var schoolId = school ?? (int)SchoolId;
            var invoices = await _invoiceService.GetAllInvoiceOfSchoolAsync(filter, _uriService, route, schoolId,
                invoiceStatus, startDate, endDate);

            return Ok(invoices);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-newest-invoice/{subscriptionId}")]
        public async Task<IActionResult> GetNewestInvoice(int subscriptionId)
        {
            var invoice = await _invoiceService.GetNewestInvoice(subscriptionId);

            return HandleSuccess(invoice);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-invoice/{id}")]
        public async Task<IActionResult> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);

            return HandleSuccess(invoice);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-multiple-invoices")]
        public async Task<IActionResult> DeleteMultipleInvoiceAsync([FromBody] int[] ids)
        {
            var result = await _invoiceService.DeleteMultipleInvoiceAsync(ids);

            return HandleSuccess(result);
        }
    }
}