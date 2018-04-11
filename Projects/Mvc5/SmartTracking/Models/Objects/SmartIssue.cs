using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTracking.Models.Objects
{
    public class SmartIssue
    {
        public string IssueTitle { get; set; }
        public string IssueDescription { get; set; }
        public int ProjectId { get; set; }
        public int? IssueCategoryId { get; set; }
        public int? IssueStatusId { get; set; }
        public int? IssuePriorityId { get; set; }
        public int? IssueMilestoneId { get; set; }
        public int? IssueAffectedMilestoneId { get; set; }
        public int? IssueTypeId { get; set; }
        public int? IssueResolutionId { get; set; }
        public string IssueAssignedUserName { get; set; }
        public string IssueCreatorUserName { get; set; }
        public string IssueOwnerUserName { get; set; }
        public DateTime? IssueDueDate { get; set; }
        public int IssueVisibility { get; set; }
        public decimal IssueEstimation { get; set; }
        public int IssueProgress { get; set; }
    }
}