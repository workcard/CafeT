using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTracking.Helpers
{
    public class DataChartHelpers
    {
        public static string GetStaffLineBarChart(List<GraphItem> performances)
        {
            string result = "\'";
            int i = 0;

            foreach (var item in performances)
            {
                if (i > 0)
                    result += ",";
                result += "{ \"date\": \"" + item.x + "\", \"performance\": " + item.y+ " }";
                i++;
            }

            result += "\'";
            return result;
        }
    }
}