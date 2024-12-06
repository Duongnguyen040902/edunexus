using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers;

public class InvoiceManagerController : BaseApiController<InvoiceManagerController>
{
    private readonly IInvoiceService _invoiceService;
    private readonly IUriService _uriService;

    public InvoiceManagerController(ILogger<InvoiceManagerController> logger,
        IInvoiceService invoiceService,
        IUriService uriService
    ) : base(logger)
    {
        _invoiceService = invoiceService;
        _uriService = uriService;
    }

    [HttpGet("all-invoice")]
    public async Task<IActionResult> GetAllInvoiceAsync([FromQuery] PaginationFilter filters, int? status,
        string? keyword)
    {
        var route = Request.Path.Value;

        var invoices = await _invoiceService.GetAllInvoicesAsync(filters, _uriService, route, status, keyword);

        return Ok(invoices);
    }

    [HttpGet("get-invoice/{id}")]
    public async Task<IActionResult> GetInvoiceAsync(int id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);

        return HandleSuccess(invoice);
    }

    [HttpPost("create-invoice")]
    public async Task<IActionResult> CreateInvoiceAsync([FromBody] CreateInvoiceDTO createInvoiceDto)
    {
        if (!ModelState.IsValid)
        {
            return HandleModelStateErrors(ModelState);
        }
        
        var createdInvoice = await _invoiceService.CreateInvoiceAsync(createInvoiceDto);

        return HandleSuccess(createdInvoice);
    }

    [HttpPut("update-invoice/{id}")]
    public async Task<IActionResult> UpdateInvoiceAsync(int id, [FromBody] CreateInvoiceDTO updateInvoiceDTO)
    {
        if (!ModelState.IsValid)
        {
            return HandleModelStateErrors(ModelState);
        }

        var updatedInvoice = await _invoiceService.UpdateInvoiceAsync(updateInvoiceDTO, id);

        return HandleSuccess(updatedInvoice);
    }

    [HttpDelete("delete-multiple-invoices")]
    public async Task<IActionResult> DeleteInvoiceAsync([FromBody] int[] ids)
    {
        var result = await _invoiceService.DeleteMultipleInvoiceAsync(ids);

        return HandleSuccess(result);
    }

    [HttpPost("schedule-invoice")]
    public async Task<IActionResult> ScheduleInvoiceAsync()
    {
        var result = await _invoiceService.ScheduleInvoiceAsync(); //TODO:send with invoice

        return HandleSuccess(result);
    }
    
    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePaymentAsync([FromBody] PaymentDTO createPaymentDto)
    {
        if (!ModelState.IsValid)
        {
            return HandleModelStateErrors(ModelState);
        }

        var createdPayment = await _invoiceService.CreatePaymentAsync(createPaymentDto);

        return HandleSuccess(createdPayment);
    }
}