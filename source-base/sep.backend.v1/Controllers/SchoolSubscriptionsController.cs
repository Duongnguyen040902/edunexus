using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;
using System.ComponentModel.DataAnnotations;

namespace sep.backend.v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolSubscriptionsController : BaseApiController<SchoolSubscriptionsController>
    {
        private readonly ISchoolSubscriptionService _schoolSubscriptionService;
        private readonly IUriService _uriService;

        public SchoolSubscriptionsController(ILogger<SchoolSubscriptionsController> logger, ISchoolSubscriptionService schoolSubscriptionService, IUriService uriService) : base(logger)
        {
            _schoolSubscriptionService = schoolSubscriptionService;
            _uriService = uriService;
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSubscriptionsOfSchool([FromQuery] PaginationFilter filter,
            int? status = null,
            int? year = null)
        {
            var route = Request.Path.Value;
            var subscriptionsResponse = await _schoolSubscriptionService.GetAllSubscriptionsOfSchoolAsync(filter, _uriService, route, (int)SchoolId, status, year);
            return Ok(subscriptionsResponse);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentSubscriptionOfSchool()
        {
            var subscription = await _schoolSubscriptionService.GetCurrentSubscriptionOfSchoolAsync((int)SchoolId);                      
            return HandleSuccess(subscription);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create-invoice/{subscriptionPlanId}")]
        public async Task<IActionResult> CreateInvoice(int subscriptionPlanId)
        {
            var invoiceId = await _schoolSubscriptionService.CreateInvoiceForSubscriptionAsync((int)SchoolId, subscriptionPlanId);
            return HandleSuccess(invoiceId);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("generate-payment-url/{invoiceId}")]
        public async Task<IActionResult> GeneratePaymentUrl(int invoiceId)
        {
            var paymentUrl = await _schoolSubscriptionService.GeneratePaymentUrlAsync(invoiceId);
            return HandleSuccess(paymentUrl);
        }

        [HttpGet("payment-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentCallback()
        {
            var success = await _schoolSubscriptionService.ProcessPaymentCallbackAsync(HttpContext);
            if (success)
            {
                return Redirect("http://localhost:8082/school/subscription/manager?status=success");
            }
            else
            {
                return Redirect("http://localhost:8082/school/subscription/manager?status=failure");
            }
        }

    }
}
