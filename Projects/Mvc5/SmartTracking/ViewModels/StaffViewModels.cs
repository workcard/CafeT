using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartTracking.ViewModels
{    
    public class StaffTimesViewModel
    {
        [Display(Name = "FromDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }
        public string UserName { get; set; }

        public List<GraphItem> Performances { get; set; }
        public List<IssueViewModel> IssuesNotFinished { get; set; }
        public List<IssueViewModel> IssuesFuture { get; set; }
        public List<IssueViewModel> IssuesAll { get; set; }
    }

}