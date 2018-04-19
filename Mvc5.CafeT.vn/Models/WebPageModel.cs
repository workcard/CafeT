﻿using CafeT.BusinessObjects;
using CafeT.Html;
using HtmlAgilityPack;
using System;

namespace Mvc5.CafeT.vn.Models
{
    public class WebPageModel : WebPageObject
    {
        HtmlParseError Errors { set; get; }
        public WebPageModel():base()
        {
        }
        public WebPageModel(string url):base(url)
        {
            Remove();
        }
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
            //this.Page.HtmlSmart.Document.RemoveScriptsAndStyles();
        }
    }
}