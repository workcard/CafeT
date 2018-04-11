using CafeT.GoogleServices;
using CafeT.Html;
using CafeT.Translators;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CafeT.Text;

namespace ChuyenTin.vn.Controllers
{
    public class HomeController : Controller
    {
        private string UserName = "taipm.vn@gmail.com";
        private string apiKey = "AIzaSyBtikadgAQovJou8j4fOWaZsOjm-21K2gM";
        private string apiKeyName = "ChuyenTin.vn";
        string appName = "ChuyenTin";
        string userName = "taipm.vn@gmail.com";

        public HomeController()
        {
            
        }

        public ActionResult Search()
        {
            GoogleServices services = new GoogleServices();
            var results = services.Search("C#").Items;
            return View("Messages/_GoogleResults", results);
        }

        public ActionResult SearchImages()
        {
            string secretFile = Server.MapPath("~/App_Data/client_secrets.json");
            GoogleServices services = new GoogleServices();
            var results = services.SearchImage("C#").Items;
            ViewBag.SecrectFile = secretFile;
            return View("Messages/_GoogleImageResults", results);
        }
        public ActionResult Translate()
        {
            Translator translator = new Translator();
            ViewBag.Words = translator.Translate("Hello, how are you");
            return View("Messages/_HtmlString", ViewBag.Words);
        }


        public ActionResult Index(int? n)
        {
            string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTo0l8CL4vGeZl2t9Oj0d0g_ZavcGblmiNoym2aemYMxFfF-wLB9l0Pk3ngpPJAkXHG9w_6ZRIbGzDo/pubhtml?gid=1447858291&single=true";
            var web = new WebPage(url);
            ViewBag.Links = web.Links;
            ViewBag.Images = web.Images;
            return View("Index");
        }
        public ActionResult Details(string url)
        {
            var web = new WebPage(url);
            var links = web.Links.Where(t => t.Contains(url))
                .ToList();
            List<string> images = new List<string>();
            foreach(var link in links)
            {
                var page = new WebPage(link);
                var pageImages = page.Images;
                foreach(var _image in pageImages)
                {
                    if(_image.Contains("?imgmax"))
                    {
                        images.Add(_image.GetFromBeginTo("?imgmax"));
                    }
                }
            }
            ViewBag.Images = images;
            return View("Details");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}