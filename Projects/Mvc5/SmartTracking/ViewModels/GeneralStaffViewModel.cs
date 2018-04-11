using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.ViewModels
{
    public class GeneralStaffViewModel
    {
        public GraphItem PerfomanceLastWorkingDay { get; set; }
        public GraphItem PerfomanceToday { get; set; }
        public GraphItem PerfomanceNextWorkingDay { get; set; }

        public List<IssueViewModel> IssuesNotFinished { get; set; }
        public List<IssueViewModel> IssuesFuture { get; set; }
    }
}
