using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartTracking.ViewModels
{
    public class GeneralCIOViewModel
    {
        public GraphItem PerfomanceAverageLastWorkingDay { get; set; }
        public GraphItem PerfomanceAverageToday { get; set; }
        public GraphItem PerfomanceAverageNextWorkingDay { get; set; }
        public GraphItem PerfomanceAverageThisWeek { get; set; }

        public List<GraphItem> PerformancesLastWorkingDate { get; set; }
        public List<GraphItem> PerformancesToday { get; set; }
        public List<GraphItem> PerformancesNextWorkingDate { get; set; }
        public List<IssueViewModel> IssuesNotFinished { get; set; }
        public List<IssueViewModel> IssuesFuture { get; set; }
    }

    public class ListPerformanceUsersViewModel
    {
        public DateTime Date { get; set; }
        public List<GraphItem> PerformancesUsers { get; set; }
    }

    public class CIOTimesViewModel
    {
        [Display(Name = "FromDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }

        public List<ListPerformanceUsersViewModel> ListPerformances { get; set; }
    }

    

    public class CIOListIssuesForDateViewModel
    {
        [Display(Name = "FromDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }

        public List<ListIssuesForDateViewModel> ListIssuesForDate { get; set; }
    }

    public class OrderTimesViewModel
    {
        public string PhaseName { get; set; }
        public List<GraphItem> PerformancesUsers { get; set; }
    }
}