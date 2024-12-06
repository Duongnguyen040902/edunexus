using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : BaseApiController<SubscriptionPlanController>
    {
        private readonly ISubscriptionPlanService _subscriptionService;
        private readonly IUriService _uriService;

        public SubscriptionPlanController(ILogger<SubscriptionPlanController> logger, ISubscriptionPlanService subscriptionService, IUriService uriService) : base(logger)
        {
            _subscriptionService = subscriptionService;
            _uriService = uriService;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-subscription")]
        public async Task<IActionResult> GetSubscriptionAsync()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            return HandleSuccess(subscriptions);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-subscription/{id}")]
        public async Task<IActionResult> GetSubscriptionDetailAsync(int id)
        {
            var subscriptions = await _subscriptionService.GetSubscriptionsDetailAsync(id);
            return HandleSuccess(subscriptions);
        }
    }
}
