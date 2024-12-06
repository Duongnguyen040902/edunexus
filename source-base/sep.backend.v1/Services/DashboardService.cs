using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAutoMapper _mapper;

    public DashboardService(IUnitOfWork unitOfWork, IAutoMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenueAsync(DashboardDto dashboardDto)
    {
        var query = _unitOfWork.GetRepository<Payment>()
            .Where(p => p.PaymentDate.Year == dashboardDto.Year && p.Status == (int)PaymentStatuses.Success);
        DateTime now = DateTime.Now;
        switch (dashboardDto.FilterType?.ToLower())
        {
            case "month":
                query = query.Where(p => p.PaymentDate >= new DateTime(now.Year, now.Month, 1));
                break;
            case "quarter":
                int quarterStartMonth = ((now.Month - 1) / 3) * 3 + 1;
                query = query.Where(p => p.PaymentDate >= new DateTime(now.Year, quarterStartMonth, 1));
                break;
            case "year":
                query = query.Where(p => p.PaymentDate >= new DateTime(now.Year, 1, 1));
                break;
        }

        var monthlyRevenues = await query
            .GroupBy(p => p.PaymentDate.Month)
            .Select(group => new { Month = group.Key, TotalRevenue = group.Sum(p => p.Amount) })
            .ToListAsync();

        var fullYearData = Enumerable.Range(1, 12)
            .Select(month => new MonthlyRevenueDto
            {
                Month = month,
                TotalRevenue = monthlyRevenues.FirstOrDefault(m => m.Month == month)?.TotalRevenue ?? 0
            })
            .ToList();

        return fullYearData;
    }

    public async Task<TotalUserSchoolDto> GetUserStatusCountsAsync(DashboardDto dashboardDto)
    {
        var query = _unitOfWork.GetRepository<School>().Where(s => s.CreatedDate.Year == dashboardDto.Year);

        DateTime now = DateTime.Now;

        switch (dashboardDto.FilterType?.ToLower())
        {
            case "month":
                query = query.Where(s => s.CreatedDate >= new DateTime(now.Year, now.Month, 1));
                break;
            case "quarter":
                int quarterStartMonth = ((now.Month - 1) / 3) * 3 + 1;
                query = query.Where(s => s.CreatedDate >= new DateTime(now.Year, quarterStartMonth, 1));
                break;
            case "year":
                query = query.Where(s => s.CreatedDate >= new DateTime(now.Year, 1, 1));
                break;
        }

        int activeCount = await query.CountAsync(s => s.AccountStatus == (int)Statuses.Active);
        int inactiveCount = await query.CountAsync(s => s.AccountStatus == (int)Statuses.Inactive);

        return new TotalUserSchoolDto
        {
            DataStatus = new Dictionary<string, int>
            {
                { "Hoạt động", activeCount },
                { "Ngừng hoạt động", inactiveCount }
            },
            Total = activeCount + inactiveCount,
        };
    }

    public async Task<IEnumerable<RevenueSubscriptionDto>> GetTotalSubscriptionAsync(DashboardDto dashboardDto)
    {
        var query = _unitOfWork.GetRepository<SchoolSubscriptionPlan>()
            .Where(s => s.StartDate.Year == dashboardDto.Year);

        DateTime now = DateTime.Now;

        switch (dashboardDto.FilterType?.ToLower())
        {
            case "month":
                query = query.Where(sp => sp.StartDate >= new DateTime(now.Year, now.Month, 1));
                break;
            case "quarter":
                int quarterStartMonth = ((now.Month - 1) / 3) * 3 + 1;
                query = query.Where(sp => sp.StartDate >= new DateTime(now.Year, quarterStartMonth, 1));
                break;
            case "year":
                query = query.Where(sp => sp.StartDate >= new DateTime(now.Year, 1, 1));
                break;
        }

        var subscriptionCounts = await query
            .GroupBy(s => s.SubscriptionPlan.Name)
            .Select(group => new RevenueSubscriptionDto
            {
                PlanName = group.Key,
                TotalSubscription = group.Count()
            })
            .ToListAsync();

        return subscriptionCounts;
    }
}