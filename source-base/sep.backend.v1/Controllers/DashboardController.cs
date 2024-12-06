using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers;

public class DashboardController : BaseApiController<DashboardController>
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(ILogger<DashboardController> logger, IDashboardService dashboardService) : base(logger)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("get-revenue")]
    public async Task<IActionResult> GetRevenueAsync([FromQuery] DashboardDto dashboardDto)
    {
        var monthlyRevenues = await _dashboardService.GetMonthlyRevenueAsync(dashboardDto);

        return HandleSuccess(monthlyRevenues);
    }

    [HttpGet("get-total-school")]
    public async Task<IActionResult> GetTotalSchoolAsync([FromQuery] DashboardDto dashboardDto)
    {
        var userStatusCounts = await _dashboardService.GetUserStatusCountsAsync(dashboardDto);

        return HandleSuccess(userStatusCounts);
    }

    [HttpGet("get-total-subscription")]
    public async Task<IActionResult> GetTotalSubscriptionAsync([FromQuery] DashboardDto dashboardDto)
    {
        var totalSubscriptions = await _dashboardService.GetTotalSubscriptionAsync(dashboardDto);

        return HandleSuccess(totalSubscriptions);
    }
}