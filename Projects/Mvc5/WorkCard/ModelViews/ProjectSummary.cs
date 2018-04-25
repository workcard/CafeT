using CafeT.Enumerable;
using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.ModelViews
{
    public class ProjectSummary
    {
        public Project ProjectModel { set; get; }
        public IEnumerable<WorkIssue> Issues { set; get; }
        public double TimeToDo { set; get; }
        public double Cost { set; get; }
        public IEnumerable<WorkIssue> CompletedIssues { set; get; }
        public IEnumerable<WorkIssue> NewIssues { set; get; }
        public IEnumerable<WorkIssue> LastIssues { set; get; } //default take(10)
        public ProjectSummary(Project project, IEnumerable<WorkIssue> issues)
        {
            ProjectModel = project;
            Issues = issues;
            Refresh();
        }

        public void Refresh()
        {
            if(Issues != null && Issues.Count()>0)
            {
                Cost = Issues.Sum(t => t.Price);
                TimeToDo = Issues.Sum(t => t.IssueEstimation);
                CompletedIssues = Issues.Where(t => t.IsCompleted());
                NewIssues = Issues.Where(t => t.Status == CafeT.Objects.Enums.IssueStatus.New);
                LastIssues = Issues.OrderByDescending(t => t.CreatedDate).TakeMax(10);
            }
        }
    }
}