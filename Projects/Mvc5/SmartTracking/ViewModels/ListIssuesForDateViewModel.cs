using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.ViewModels
{
    public class ListIssuesForDateViewModel
    {
        public DateTime Date { get; set; }
        public List<IssueViewModel> Issues { get; set; }
    }
}
