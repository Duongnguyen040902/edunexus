using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers;

public class ServicePackageController : BaseApiController<ServicePackageController>
{
    private readonly ISubscriptionPlanService _subscriptionService;

    public ServicePackageController(ILogger<ServicePackageController> logger,
        ISubscriptionPlanService subscriptionService) : base(logger)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet("get-all-service-package")]
    public async Task<IActionResult> GetAllServicePackage()
    {
        var servicePackages = await _subscriptionService.GetAllSubscriptionsByAdminAsync();
        return HandleSuccess(servicePackages);
    }

    [HttpGet("get-service-package-by-id/{id}")]
    public async Task<IActionResult> GetServicePackageById(int id)
    {
        var servicePackage = await _subscriptionService.GetSubscriptionByIdAsync(id);

        return HandleSuccess(servicePackage);
    }

    [HttpPost("create-service-package")]
    public async Task<IActionResult> CreateServicePackage([FromBody] SubscriptionPlanDTO servicePackage)
    {
        if (!ModelState.IsValid)
        {
            return HandleModelStateErrors(ModelState);
        }

        var createdPackage = await _subscriptionService.CreateSubscriptionAsync(servicePackage);

        return HandleSuccess(createdPackage);
    }

    [HttpPut("update-service-package/{id}")]
    public async Task<IActionResult> UpdateServicePackage(int id, [FromBody] SubscriptionPlanDTO servicePackage)
    {
        if (!ModelState.IsValid)
        {
            return HandleModelStateErrors(ModelState);
        }
        var updateSubscriptionAsync = await _subscriptionService.UpdateSubscriptionAsync(id, servicePackage);

        return HandleSuccess(updateSubscriptionAsync);
    }
}