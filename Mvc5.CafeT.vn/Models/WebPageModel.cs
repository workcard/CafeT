
using CafeT.Html;
using HtmlAgilityPack;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Models;

namespace Mvc5.CafeT.vn.Models
{
    public class WebPageModel : BaseObject
    {
        [NotMapped]
        public WebPage Page { set; get; }

        public string Url { set; get; }
        public string Title { set; get; }
        public string Meta { set; get; }
        public string HtmlContent { set; get; }

        public string[] NodesCss { set; get; }

        public WebPageModel() : base()
        {
            Title = string.Empty;
            Url = string.Empty;
            Page = new WebPage();
        }
        public WebPageModel(string url) : base()
        {
            Page = new WebPage(url);
            Title = Page.Title;
            Url = url;
            HtmlContent = Page.HtmlContent;
            Remove();
        }

        HtmlParseError Errors { set; get; }
        
        public void Remove()
        {
            try
            {
                Page.HtmlContent.RemoveScriptsAndStyles();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}