using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTracking.ViewModels
{
    public class StaffModelView
    {
        public List<IssueViewModel> _NotStandardIssues {get;set;}
        public List<IssueViewModel> _TodayIssues { set; get; }
        public List<IssueViewModel> _ExpiredIssues { set; get; }
        public List<IssueViewModel> _LastWorkingIssues { set; get; }
        public List<IssueViewModel> _NextWorkingdayIssues { set; get; }

        public StaffModelView()
        {
            _ExpiredIssues = new List<IssueViewModel>();
            _LastWorkingIssues = new List<IssueViewModel>();
            _NextWorkingdayIssues = new List<IssueViewModel>();
            _TodayIssues = new List<IssueViewModel>();
            _NotStandardIssues = new List<IssueViewModel>();
        }
    }
}