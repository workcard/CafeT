using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.ModelViews
{

    public class IssuesView
    {
        public IEnumerable<WorkIssue> Issues { set; get; }
        public IEnumerable<WorkIssue> TodayIssues { set; get; }
        public IEnumerable<WorkIssue> YesterdayIssues { set; get; }
        public IEnumerable<WorkIssue> TomorrowIssues { set; get; }
        public IEnumerable<WorkIssue> OtherIssues { set; get; }

        public IssuesView(IEnumerable<WorkIssue> issues)
        {
            Issues = issues;
            TotalTimes = Issues.Sum(t => t.IssueEstimation);
        }

        public double TotalTimes { set; get; }
        

        public List<WorkIssue> GetToday()
        {
            return Issues.Where(t => t.IsToday()).ToList();
        }

        public List<WorkIssue> GetTomorrow()
        {
            return Issues.Where(t => t.IsTomorrow()).ToList();
        }

        public List<WorkIssue> GetYesterday()
        {
            return Issues.Where(t => t.IsYesterday()).ToList();
        }

        public List<WorkIssue> GetCreatedBy(string userName)
        {
            return Issues.Where(t => t.CreatedBy.ToLower() == userName.ToLower()).ToList();
        }

        public List<WorkIssue> GetUpdatedBy(string userName)
        {
            return Issues.Where(t => t.UpdatedBy.ToLower() == userName.ToLower()).ToList();
        }
    }
}