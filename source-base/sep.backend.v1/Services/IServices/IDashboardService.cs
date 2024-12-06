using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices;

public interface IDashboardService
{
    Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenueAsync(DashboardDto dashboardDto);
    Task<TotalUserSchoolDto> GetUserStatusCountsAsync(DashboardDto dashboardDto);
    Task<IEnumerable<RevenueSubscriptionDto>> GetTotalSubscriptionAsync(DashboardDto dashboardDto);
}