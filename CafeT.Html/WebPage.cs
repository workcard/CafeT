using CafeT.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CafeT.Html
{
    public class HtmlTable
    {
        public string Name { set; get; }
        public string HtmlContent { set; get; }
        public List<string> Rows { set; get; } = new List<string>();
        public List<string> Columns { set; get; } = new List<string>();
        public HtmlTable(string htmlContent)
        {
            HtmlContent = htmlContent;
            GetRows();
        }
        public void GetRows()
        {
            HtmlDocument _doc = new HtmlDocument();
            _doc.LoadHtml(HtmlContent);
            var _rows = _doc.DocumentNode.SelectNodes("//table//tr");
            if (_rows != null && _rows.Count > 0)
            {
                Name = _rows[0].InnerText;
                foreach (var _row in _rows)
                {
                    Rows.Add(_row.InnerText);
                }
            }
        }
        public bool MaybeRate()
        {
            if (Rows.Where(t => t.IsContainsNumber()).Count() > 3) return true;
            return false;
        }
    }

    public class WebPage
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument html;
        //public SmartHtml HtmlSmart { set; get; }
        public string Domain { set; get; }
        public string PathConfig { set; get; }
        public string Url { set; get; }
        public string Title { set; get; }
        public string Meta { set; get; }
        public string HtmlContent { set; get; }
        public string TextContent { set; get; }
        public List<string> CssDivs { set; get; } = new List<string>();
        public List<string> CssClasses { set; get; } = new List<string>();
        public List<string> Images { set; get; } = new List<string>();
        public List<HtmlTable> HtmlTables { set; get; } = new List<HtmlTable>();
        public List<string> Links { set; get; } = new List<string>();
        public List<string> InternalLinks { set; get; } = new List<string>();
        public List<string> ExternalLinks { set; get; } = new List<string>();
        
        public List<string> Keywords { set; get; } = new List<string>(); //Keyword start with #
        HtmlParseError Errors { set; get; }

        public string[] NodesCss { set; get; }

        public WebPage() 
        {
            Title = string.Empty;
        }

        public WebPage(string url)
        {
            if(url.IsUrl())
            {
                Url = url;
                Domain = GetDomain();
                web = new HtmlWeb();
                html = web.Load(Url);
                HtmlContent = html.DocumentNode.OuterHtml;
                TextContent = HtmlContent.HtmlToText();
                var _allLinks = HtmlContent.GetUrls();
                GetHtmlTables();
                BuildKeywords();
                CssDivs = HtmlContent.GetAllDivs().ToList();
                


                foreach (string _link in _allLinks)
                {
                    if (_link.StartsWith("/"))
                    {
                        string _url = Url.GetBefore(Domain) + Domain + _link;
                        Links.Add(_url);
                    }
                    else
                    {
                        Links.Add(_link);
                    }
                }
                if(Links != null)
                {
                    InternalLinks = Links.Where(t => t.StartsWith(Url.GetBefore(Domain) + Domain)).ToList();
                }
                GetImages("jpg");
            }
        }
        public void BuildKeywords()
        {
            #region Url
            if (Url.ToLower().Contains("truyen"))
            {
                Keywords.Add("#image"); //Demo
            }
            else if(Url.IsImageUrl())
            {
                Keywords.Add("#image"); //Demo
            }
            #endregion
            #region TextContent
            if (HtmlContent.ToLower().Contains("Ngân hàng".ToLower())
                || HtmlContent.ToLower().Contains("bank".ToLower())
                )
            {
                Keywords.Add("#table"); //Demo
            }
            else if (HtmlContent.ToLower().Contains("tỷ giá"))
            {
                Keywords.Add("#table"); //Demo
            }
            else if (HtmlContent.ToLower().Contains("lãi suất".ToLower()))
            {
                Keywords.Add("#table"); //Demo
            }
            else if (HtmlContent.ToLower().Contains("vietlott".ToLower()))
            {
                Keywords.Add("#table"); //Demo
            }
            else if (HtmlContent.ToLower().Contains("xổ số".ToLower()))
            {
                Keywords.Add("#table"); //Demo
            }
            else if (HtmlContent.ToLower().Contains("bảng giá".ToLower()))
            {
                Keywords.Add("#table"); //Demo
            }
            else if (HtmlContent.ToLower().Contains("hàng hóa".ToLower()))
            {
                Keywords.Add("#table"); //Demo
            }
            #endregion

            Keywords = Keywords.Distinct().ToList();
        }

        public void GetHtmlTables()
        {
            var tables = html.DocumentNode.SelectNodes("//table");
            if (tables != null)
            {
                foreach (string table in tables.Select(t=>t.OuterHtml))
                {
                    HtmlTable _table = new HtmlTable(table);
                    HtmlTables.Add(_table);
                }
                HtmlTables = HtmlTables.Distinct().ToList();
            }
        }

        public string GetDomain()
        {
            Uri myUri = new Uri(Url);
            string host = myUri.Host;
            return host;
        }

        //public void LoadContent()
        //{
        //    string _minTag = HtmlSmart.MeaningNodes
        //        .Where(t => t.InnerText.ToStandard().GetCountWords() > 100)
        //        .Select(t => t.OuterHtml).FirstOrDefault();
        //    HtmlContent = _minTag;
        //}

        //public void LoadTitle()
        //{
        //    if (HtmlSmart.Nodes == null || HtmlSmart.Nodes.Count() == 0)
        //    {
        //        Title = string.Empty;
        //    }
        //    string _minTag = HtmlSmart.Nodes.Where(t => t.CanTitle())
        //        .Where(t => t.InnerText.ToStandard().GetCountWords() > 5)
        //        .Select(t => t.InnerText.ToStandard()).OrderBy(t => t.GetCountWords()).FirstOrDefault();
        //    Title = _minTag;
        //}

        public List<HtmlNode> GetNodesByClass(string className)
        {
            return html.DocumentNode.GetNodesByClasses(className);
        }
        public void GetImages(string ext)
        {
            var imageLinks = Links.Where(t => t.Contains(ext)).ToList();
            try
            {
                var images = HtmlContent.GetImages().Distinct().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Images = imageLinks;
        }
        //public void GetNodeBy(string className)
        //{
        //    var allElementsWithClassFloat =
        //        html.DocumentNode.SelectNodes("//*[contains(@class,'float')]");
        //}
    }
}
