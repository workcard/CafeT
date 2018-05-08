using CafeT.Enumerable;
using CafeT.Objects.Enums;
using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.ModelViews
{
    public class ProjectSummary
    {
        public Project ProjectModel { set; get; }
        public double TimeToDo { set; get; }
        public double Cost { set; get; }
        public List<WorkIssue> Issues { set; get; } = new List<WorkIssue>();
        public List<WorkIssue> CompletedIssues { set; get; } = new List<WorkIssue>();
        public List<WorkIssue> NewIssues { set; get; } = new List<WorkIssue>();
        public List<WorkIssue> LastIssues { set; get; }  = new List<WorkIssue>();
        public ProjectSummary(Project project, List<WorkIssue> issues)
        {
            ProjectModel = project;
            Issues = issues;
            Refresh();
        }

        public void Refresh()
        {
            if(Issues != null && Issues.Any())
            {
                Cost = Issues.Sum(t => t.Price);
                TimeToDo = Issues.Sum(t => t.IssueEstimation);
                CompletedIssues = Issues.Where(t => t.IsCompleted()).ToList();
                NewIssues = Issues.Where(t => t.Status == IssueStatus.New).ToList();
                LastIssues = Issues.OrderByDescending(t => t.CreatedDate).TakeMax(10).ToList();
            }
        }
    }
}