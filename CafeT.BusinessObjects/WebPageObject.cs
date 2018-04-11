using CafeT.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects
{
    public class WebPageObject : BaseObject
    {
        [NotMapped]
        public WebPage Page { set; get; }
        
        public string Url { set; get; }
        public string Title { set; get; }
        public string Meta { set; get; }
        public string HtmlContent { set; get; }
        
        public string[] NodesCss { set; get; }

        public WebPageObject() : base()
        {
            Title = string.Empty;
            Url = string.Empty;
            Page = new WebPage();
        }
        public WebPageObject(string url) : base()
        {
            Page = new WebPage(url);
            Title = Page.Title;
            Url = url;
            HtmlContent = Page.HtmlContent;
        }
    }
}
