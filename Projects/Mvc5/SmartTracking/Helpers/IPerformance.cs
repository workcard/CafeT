using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.Helpers
{
    interface IPerformance
    {
        decimal PerformanceOnDay();
        decimal PerformanceOfStaff(string userName, DateTime date);
        decimal PerformanceOfStaff(string userName, DateTime fromDate, DateTime endDate);
    }
}
