using System.Collections.Generic;

namespace Web.ModelViews
{
    public class IndexIssuesViewModel
    {
        public IEnumerable<IssuesView> Items { get; set; }
        public IEnumerable<IssuesView> TodayItems { set; get; }
        public IEnumerable<IssuesView> NextItems { set; get; } //Net working day
        public IEnumerable<IssuesView> NextWeekItems { set; get; } //Net working day
        public IEnumerable<IssuesView> ThisWeekItems { set; get; } //Net working day
        public IEnumerable<IssuesView> YesterdayItems { set; get; } //Net working day

        public IndexIssuesViewModel(IEnumerable<IssuesView> issues)
        {
            Items = issues;
            //TodayItems = Items.GetToday();
            //NextItems = Items.GetInDay(DateTime.Today.AddWorkdays(1));
            //YesterdayItems = Items.GetInDay(DateTime.Today.AddWorkdays(-1));
        }
        //public Pager Pager { get; set; }
    }
}