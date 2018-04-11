using System;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class GraphViewModel
    {
        List<GraphItem> Items { set; get; }
        public GraphViewModel()
        {
            Items = new List<GraphItem>();
        }        
    }

    public class ColumnGraphViewModel
    {
        List<ColumnGraphItem> Items { set; get; }
        public ColumnGraphViewModel()
        {
            Items = new List<ColumnGraphItem>();
        }
    }

    public class ColumnGraphItem
    {
        public decimal Performance { get; set; }
        public string Note { get; set; }
    }

    public class PerfomanceInDateViewModel
    {
        public DateTime Date { get; set; }
        public decimal Performance { get; set; }
        public string Note { get; set; }
    }

    public class ColumnGraphItemForUser
    {
        public string UserName { get; set; }
        public decimal Performance { get; set; }
        public string Note { get; set; }
    }

    public class GraphItem
    {
        public string x { get; set; }
        public decimal y { get; set; }
        public string Note { get; set; }
    }
}
