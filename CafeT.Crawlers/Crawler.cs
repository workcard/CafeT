using CafeT.DataMining;
using BusinessObjects;
using GenericObjects.Models;
using HtmlAgilityPack;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CafeT.Helpers;

namespace IdentitySample
{
    public class WebPage
    {
        public string url { set; get; }
        public string HtmlContent { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
        public List<string> FileLinks { set; get; }
        public List<string> ImageLinks { set; get; }
        public List<string> CssClasses { set; get; }
        public List<string> InternalLinks { set; get; }
        public List<string> ExternalLinks { set; get; }
    }

    public class Crawler
    {
        public string[] Urls { set; get; }
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private CrawlersDbContext db = new CrawlersDbContext();
        //protected string[] Urls { set; get; }

        public Crawler()
        {
        }

        public void Crawl()
        {
            foreach(string url in Urls)
            {
                Crawl(url);
            }
            //Urls = this.GetUrls();
            //var _articles = this.ParseUrls(Urls);
            //this.Process(_articles);
        }

        public void Crawl(string url)
        {
            //Tiến hành đọc nội dung, phân tích và đổ vào khung Url để tiến hành xử lý.
            //UrlCss
            //UrlFilter
            //Url
            List<GenericArticle> _articles = ParseUrl(url);
        }

        //public string[] GetUrls()
        //{
        //    string[] _urls = db.Urls.Where(c => c.Enable).Select(c => c.UrlLink).ToArray();
        //    return _urls;
        //}

        //public List<ArticleBo> ParseUrls(string[] urlLinks)
        //{
        //    List<ArticleBo> _articles = new List<ArticleBo>();
        //    if (urlLinks.Length > 0)
        //    {
        //        foreach (string _urlLink in urlLinks)
        //        {
        //            var _items = ParseUrl(_urlLink).Distinct().ToList();
        //            _articles.AddRange(_items);
        //        }
        //    }
        //    return _articles;
        //}

        //public void Process(List<ArticleBo> models)
        //{
        //    if (models != null && models.Count > 0)
        //        new ArticleManager().Process(models);
        //}

        private List<GenericArticle> ParseUrl(string url)
        {
            List<GenericArticle> _articles = new List<GenericArticle>();

            var _url = db.Urls.Where(c => c.UrlLink == url).FirstOrDefault();

            string _cssForItem = _url.CssClassItems;

            List<GenericArticle> _objects = GetArticles(url, _cssForItem);
            return _articles;
        }

        private WebPage ToWebPage(string url)
        {
            WebPage _page = new WebPage();
            _page.url = url;
            //
            List<GenericArticle> _articles = new List<GenericArticle>();

            var _url = db.Urls.Where(c => c.UrlLink == url).FirstOrDefault();

            string _cssForItem = _url.CssClassItems;

            List<GenericArticle> _objects = GetArticles(url, _cssForItem);
            return _page;
        }

        private List<GenericArticle> GetArticles(string url, string css)
        {
            List<GenericArticle> _articles = new List<GenericArticle>();
            GenericArticle _article = new GenericArticle();
            var _url = db.Urls.Where(c => c.UrlLink == url).FirstOrDefault();

            string _headerUrl = string.Empty;
            HtmlDocument _doc = new HtmlDocument();
            if (url.IsUrl())
            {
                _doc.LoadHtml(url.GetHtml().Result);
            }

            var _nodes = _doc.GetNodesByClass(css);

            if (_nodes != null && _nodes.Count() > 0)
            {
                foreach (var item in _nodes)
                {
                    GenericUrl _genericUrl = new GenericUrl();
                    _genericUrl = _url.ToGeneric();
                    try
                    {
                        var _object = item.HtmlNodeToGArticle(_genericUrl, _headerUrl);
                        if (_object != null)
                        {
                            _articles.Add(_object);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return _articles.ToList();
        }
    }
}