using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.ModelViews
{
    public class WorkingDay
    {
        public List<WorkIssue> Issues { set; get; }
        public string Message { set; get; } = string.Empty;

        public bool IsFull()
        {
            double _sumOfTimes = Issues.Sum(t => t.IssueEstimation);
            if (_sumOfTimes >= 8 * 60) return true;
            return false;
        }

        public bool IsEmty()
        {
            if (Issues == null && Issues.Count == 0) return true;
            return false;
        }

        public void GetMessage()
        {
            if (IsEmty())
            {
                Message = "Hiện tại bạn chưa có công việc nào.";
                return;
            }
            if(IsFull())
            {
                Message = "Hôm tại công việc của bạn quá nhiều. Nếu gấp hãy thêm và điều chỉnh công việc cho phù hợp";
                return;
            }
        }
    }
}