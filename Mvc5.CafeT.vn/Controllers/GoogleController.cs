using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using System.Linq;
using CafeT.GoogleManager;

namespace Mvc5.CafeT.vn.Controllers
{
    public class GoogleController : BaseController
    {
        //private string key { set; get; } = "AIzaSyDz0FsFxxf7xsskqxlhaNWMvxM05b4HQBc";// "AIzaSyA5naCjbPLqFdhaY6-f24bwoTwKSTHS9m4"; //CafeT.vn
        private string key { set; get; } = "AIzaSyA5naCjbPLqFdhaY6-f24bwoTwKSTHS9m4"; //CafeT.vn
        private string engine { set; get; } = "004317969426278842680:_f5wbsgg7xc";
        
        public enum GoogleCommand
        {
            Definition,
            Image,
            Keyword
        }

        Manager google;
        public GoogleController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            google = new Manager(key, engine);
        }

        public ActionResult GoogleSearch(string keywords)
        {
            string _keywords = string.Empty;
            _keywords = keywords + " site: cafet.vn";
            var results = google.Search(_keywords).Items
                .ToList();
            _keywords = keywords + " site: www.codeproject.com";
            var _items = google.Search(_keywords).Items
                .ToList();
            results.AddRange(_items);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Messages/_GoogleResults", results);
            }
            return PartialView("Messages/_GoogleResults", results);
        }

        public ActionResult GoogleImages(string keywords)
        {
            var results = google.SearchImage(keywords);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Messages/_GoogleImageResults", results.Items);
            }
            return PartialView("Messages/_GoogleImageResults", results.Items);
        }
    }
}