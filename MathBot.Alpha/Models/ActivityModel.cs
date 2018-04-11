using CafeT.BusinessObjects;

namespace MathBot.Models
{
    public class ActivityModel : BaseObject
    {
        public string Activity { set; get; }
        public ActivityModel() : base() { }
    }
}