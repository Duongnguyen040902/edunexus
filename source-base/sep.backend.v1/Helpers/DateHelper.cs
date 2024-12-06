namespace sep.backend.v1.Helpers;

public static class DateHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseDate">.</param>
    /// <param name="monthsToAdd">.</param>
    /// <returns>.</returns>
    public static DateTime AddMonthsToDate(DateTime baseDate, int monthsToAdd)
    {
        return baseDate.AddMonths(monthsToAdd);
    }
}
