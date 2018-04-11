using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.ViewModels
{
    public class IssueViewModel
    {
        public int IssueId { get; set; }
        public string IssueTitle { get; set; }
        public decimal IssueEstimation { get; set; }
        public string StatusName { get; set; }
        public string MilestoneName { get; set; }
        public string AssignedUserName { get; set; }
        public DateTime? IssueDueDate { get; set; }
        public string Notify { get; set; }
        public bool IsClosed { get; set; }
    }
}
