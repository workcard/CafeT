using System;

namespace Web.Helpers
{
    interface IPerformance
    {
        decimal PerformanceOnDay();
        decimal PerformanceOfStaff(string userName, DateTime date);
        decimal PerformanceOfStaff(string userName, DateTime fromDate, DateTime endDate);
    }
}
