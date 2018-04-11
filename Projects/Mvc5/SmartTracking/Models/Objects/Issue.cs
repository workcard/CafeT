using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.Models.Objects
{
    public class Issue
    {
        public int IssueId { get; set; }
        public string IssueTitle { get; set; }
        public string IssueDescription { get; set; }
        public string StatusName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime? IssueDueDate { get; set; }
        public decimal IssueEstimation { get; set; }
        public int IssueProgress { get; set; }
        public string MilestoneName { get; set; }
        public string AssignedUserName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdate { get; set; }
        public Guid LastUpdateUserId { get; set; }
        public bool IsClosed { get; set; }


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
        public int? IssueVisibility { get; set; }

        public bool IsExpired()
        {
            if ((IsClosed != true) && (IssueDueDate.HasValue && (IssueDueDate.Value < DateTime.Now)))
            {
                return true;
            }
            else
                return false;
        }

        public bool IsStandard()
        {
            bool _result = true;
            if (IssueTitle == null || IssueDueDate == null || IssueEstimation <= 0)
            {
                _result = false;
                
            }

            return _result;
        }

        public bool IsWarning()
        {
            if (IssueDueDate.HasValue && IssueDueDate.Value.AddDays(3) >= DateTime.Now)
            {
                return true;
            }
            else
                return false;
        }

        public string Notify()
        {
            string notify = "";
            int i = 1;
            if (IsStandard() == false)
            {
                notify += i + ".No standard ";
                i++;
            }
            if (IsExpired() == true)
            {
                notify += i + ".Expired ";
                i++;
            }
            //if (IsWarning() == false)
            //{
            //    notify += i + ". Warning ";
            //    i++;
            //}

            return notify;
        }
    }
}
