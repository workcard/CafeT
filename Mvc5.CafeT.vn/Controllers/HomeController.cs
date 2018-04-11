using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using System.Linq;
using Mvc5.CafeT.vn.ModelViews;
using System.Collections.Generic;
using CafeT.Enumerable;
using Mvc5.CafeT.vn.Models;
using PagedList;
using System.Threading.Tasks;
using CafeT.GoogleServices;
using System;

namespace Mvc5.CafeT.vn.Controllers
{
    
    public class HomeController : BaseController
    {
        private string UserName { set; get; } = "taipm.vn@gmail.com";
        public HomeController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {            
        }

        //public async Task<ActionResult> ReadDoc(int? n)
        //{
        //    Uploader service = new Uploader(UserName);
        //    service.ClientSecrectFile = Server.MapPath("~/App_Code/AppScripts.json");
        //    var files = service.ReadGoogleDoc(string.Empty);
            
        //    if (Request.IsAjaxRequest())
        //    {
        //        return (PartialView("Files/_Files", files));
        //    }
        //    return View("Files/_Files", files);
        //}

        //public async Task<ActionResult> GetNewFilesAsync(int? n)
        //{
        //    Uploader service = new Uploader(UserName);
        //    service.ClientSecrectFile = Server.MapPath("~/App_Code/client_secrets.json");
        //    var files = service.GetFilesAsync(n).Result.TakeMax(n);
        //    List<FileModel> list = new List<FileModel>();
        //    if(files != null && files.Count()>0)
        //    {
        //        foreach(var file in files)
        //        {
        //            FileModel fileModel = Mappers.Mappers.GoogleFileToModel(file);
        //            list.Add(fileModel);
        //        }
        //    }
        //    if (Request.IsAjaxRequest())
        //    {
        //        return (PartialView("Files/_Files", list));
        //    }
        //    return View("Files/_Files", list);
        //}

        public ActionResult Index()
        {
            ViewBag.Categories = _articleCategoryManager.GetAll();
            return View();
        }
        [OutputCache(Duration = 360, VaryByParam = "none")]
        public ActionResult GetNewArticles(int? n, int? page)
        {
            List<ArticleModel> _articles = new List<ArticleModel>();
            _articles = _articleManager.GetAllPublished()
                .TakeMax(n).ToList();
            var _views = _mapper.ToViews(_articles);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Articles/_NewArticles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("Articles/_NewArticles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }
        [OutputCache(Duration = 360, VaryByParam = "none")]
        public async Task<IEnumerable<WordModel>> GetNewWords(int? n)
        {
            var _words = await _unitOfWorkAsync.RepositoryAsync<WordModel>()
                .Query().SelectAsync();
                //.Where(t=>t.IsOf(User.Identity.Name))
            return  _words;
        }

        [Authorize]
        [HttpGet]
        [OutputCache(Duration = 360, VaryByParam = "none")]
        public async Task<ActionResult> GetUserProfile()
        {
            UserProfileView view = new UserProfileView();
            var words =  _unitOfWorkAsync.RepositoryAsync<WordModel>()
                .Query().Select()
                .Where(t=>t.IsOf(User.Identity.Name));

            view.CountOfNewWords = words.Where(t=>!t.IsRemembered).Count();
            view.CountOfRemembered = words.Where(t => t.IsRemembered).Count(); ;

            if(Request.IsAjaxRequest())
            {
                return PartialView("Account/_UserProfile", view);
            }
            return View("Account/_UserProfile", view);
        }

        [OutputCache(Duration = 360, VaryByParam = "none")]
        public ActionResult GetNewVideos(int? n)
        {
            var _youtubes = _articleManager.GetAllYouTubes();
            List<YouTubeView> _videos = new List<YouTubeView>();
            if (_youtubes != null && _youtubes.Count() > 0)
            {
                foreach (var _youtube in _youtubes)
                {
                    _videos.Add(new YouTubeView(_youtube));
                }
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("Videos/_Items", _videos);
            }
            return View("Videos/_Items", _videos);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";            
            return View();
        }
    }
}