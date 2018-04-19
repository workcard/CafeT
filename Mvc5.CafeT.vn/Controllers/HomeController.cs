using CafeT.Enumerable;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.ModelViews;
using PagedList;
using Repository.Pattern.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class HomeController : BaseController
    {
        //private string UserName { set; get; } = "taipm.vn@gmail.com";
        public HomeController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {            
        }

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