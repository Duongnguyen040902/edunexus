namespace sep.backend.v1.DTOs;

public class DashboardDto
{
    public int Year { get; set; }
    public string? FilterType { get; set; }
}

public class MonthlyRevenueDto
{
    public int Month { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class RevenueSubscriptionDto
{
    public int TotalSubscription { get; set; }
    public string PlanName { get; set; }
}

public class TotalUserSchoolDto
{
    public Dictionary<string, int> DataStatus { get; set; }
    public int Total { get; set; }
}

public class DashboardDataDto
{
    public IEnumerable<MonthlyRevenueDto> MonthlyRevenues { get; set; }
    public TotalUserSchoolDto UserStatusCounts { get; set; }
    public IEnumerable<RevenueSubscriptionDto> TotalSubscriptions { get; set; }
}
