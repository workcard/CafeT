using CafeT.BusinessObjects;
using CafeT.Enumerable;
using CafeT.Html;
using CafeT.Objects;
using CafeT.SmartObjects;
using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.ModelViews;
using Mvc5.CafeT.vn.Services;
using PagedList;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Mvc5.CafeT.vn.Controllers
{
    public class ArticlesController : BaseController
    {
        public ArticlesController(IUnitOfWorkAsync unitOfWorkAsync, IArticleService articleService) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;            
        }

       
        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetTopViews(int? page)
        {
            var _objects = _articleManager.GetAllPublished()
                .OrderByDescending(t=>t.CountViews)
                .ToList();

            var _views = _mapper.ToViews(_objects);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Articles/_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("Articles/_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        [Authorize]        
        public ActionResult GetAllUnPublished(int? page, string searchString)
        {
            List<ArticleModel> models = new List<ArticleModel>();
            models = _articleManager.GetAllUnPublished().ToList();
            if(User.Identity.IsAuthenticated)
            {
                if (!User.IsInRole("Admin"))
                {
                    models = models.Where(t => t.IsOf(User.Identity.Name)).ToList();
                }
            }
            var _views = _mapper.ToViews(models);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Articles/_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("Articles/_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetAllPublished(int? page, string searchString)
        {
            List<ArticleModel> models = new List<ArticleModel>();
            models = _articleManager.GetAllPublished().ToList();

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                models = models.Where(s => s.IsMatch(searchString))
                            .OrderByDescending(t => t.CreatedDate)
                            .ToList();
            }
            var _views = _mapper.ToViews(models);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Articles/_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("Articles/_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        public ActionResult GetQuestions(Guid articleId)
        {
            var _objects = _articleManager.GetQuestions(articleId).ToList();
            if(Request.IsAjaxRequest())
            {
                return PartialView("Questions/_Questions", _objects);
            }
            return View("Questions/_Questions", _objects);
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetByAuthors(Guid articleId)
        {
            var _model = _articleManager.GetById(articleId);
            var _models = _articleManager.GetAllPublished(_model.CreatedBy);
            _models = _models.Where(t => t.Id != articleId)
                .OrderByDescending(t=>t.CountViews)
                .ToList();
            var _views = _mapper.ToViews(_models.ToList());

            if (Request.IsAjaxRequest())
            {
                return PartialView("Articles/_ByAuthors", _views);
            }
            return View("Articles/_ByAuthors", _views);
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetAuthors(Guid articleId)
        {
            var _model = _articleManager.GetById(articleId);
            var _authors = UserManager.FindByNameAsync(_model.CreatedBy).Result;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Articles/_ArticleAuthors", _authors);
            }
            return View("Messages/_HtmlString", _authors.About);
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public IEnumerable<WordModel> GetMemoryWords()
        {
            return _wordManager.GetAll();
        }
        
        public Dictionary<string,int[]> GetArticlesWords(Guid articleId)
        {
            ArticleModel _article = _articleManager.GetById(articleId);
            VnTextCrawler _crawler = new VnTextCrawler();
            _crawler.Run(_article.Content.HtmlToText());

            Dictionary<string, int[]> dictionary = new Dictionary<string, int[]>();
            
            foreach(var item in _crawler.Processor.CleanWordObjects)
            {
                dictionary.Add(item.Value, item.Indexs);
            }
            return dictionary;
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public async Task<ActionResult> GetEnglishWords(Guid articleId)
        {
            ArticleModel _article = _articleManager.GetById(articleId);
            VnTextCrawler _crawler = new VnTextCrawler();
            _crawler.Run(_article.Content.HtmlToText());

            var _words = _crawler.Processor
                .CleanWordObjects
                .Where(t=>t.Lang == WordLang.English)
                .Select(t => t.Value)
                .Distinct();

            var _allWords = GetMemoryWords().Select(t => t.Model.Value).Distinct();
            var _selected = _words.Where(t=> !_allWords.Contains(t));
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("Words/_NewWords", _selected);
            }
            return View("Words/_NewWords", _selected);
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult Details(Guid id, string seoName)
        {
            string _userView = string.Empty;
            if(User.Identity.IsAuthenticated)
            {
                _userView = User.Identity.Name;
            }
            ArticleModel _article = _articleManager.GetToView(id, _userView);
            var _view = _mapper.ToView(_article);

            _view.Author = UserManager.FindByNameAsync(_view.CreatedBy).Result;

            if(_view.Author != null && _view.Author.AvatarPath != null)
            {                
                _view.ImageAuthor = _view.Author.AvatarPath;
                _view.Author = UserManager.FindByNameAsync(_view.CreatedBy).Result;
            }

            if (seoName != Helpers.Extensions.SeoName(_article.Title))
                return RedirectToActionPermanent("Details", 
                    new { id = id, seoName = Helpers.Extensions.SeoName(_article.Title) });

            return View(_view);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddQuestion(Guid articleId)
        {
            QuestionModel _model = new QuestionModel();

            _model.CreatedBy = User.Identity.Name;
            _model.ArticleId = articleId;
            if(Request.IsAjaxRequest())
            {
                return PartialView("Questions/_AddQuestion", _model);
            }
            else
            {
                return View("Questions/_AddQuestion", _model);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(QuestionModel model)
        {
            try
            {
                _articleManager.AddQuestion(model);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Questions/_Questions", _questionManager.GetAllByArticle(model.ArticleId.Value));
                }
                else
                {
                    return RedirectToAction("Details", "Articles", new { id = model.ArticleId });
                }
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        [Authorize]
        public ActionResult AddAvatar(Guid articleId)
        {
            ImageLink _model = new ImageLink();
            _model.ArticleId = articleId;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Articles/_AddAvatar", _model);
            }
            else
            {
                return View("Articles/_AddAvatar", _model);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult AddAvatar(ImageLink model)
        {
            try
            {
                var _object = _articleManager.GetById(model.ArticleId.Value);
                _object.AvatarPath = model.Link;
                _object.LastUpdatedBy = User.Identity.Name;
                _articleManager.Update(_object);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("Messages/_HtmlString", "Changed avatar");
                }
                else
                {
                    return RedirectToAction("Details", "Articles", new { id = _object.Id });
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddComment(Guid articleId)
        {
            CommentModel _model = new CommentModel();
            _model.CreatedBy = User.Identity.Name;
            _model.ArticleId = articleId;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Comments/_AddComment", _model);
            }
            else
            {
                return View("Comments/_AddComment", _model);
            }
        }
        protected string AjaxView { set; get; } = string.Empty;
        
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(CommentView comment)
        {
            AjaxView = "Comments/_comments";

            try
            {
                comment.Id = Guid.NewGuid();
                comment.CreatedBy = User.Identity.Name;
                comment.CreatedDate = DateTime.Now;                

                CommentModel model = _mapper.ToModel(comment);

                if (comment.CanInsert())
                {                    
                    _articleManager.AddComment(model.ArticleId.Value, model);
                }
                else
                {
                    string _error = "Comment không đủ ngữ nghĩa";
                    return PartialView("Messages/_Message", _error);
                }

                if(Request.IsAjaxRequest())
                {
                    return PartialView(AjaxView, _commentManager.GetAllByArticle(model.ArticleId.Value));
                }
                else
                {
                    return RedirectToAction("Details", "Articles", new { id = model.ArticleId });
                }
            }
            catch(Exception ex)
            {
                HandleErrorInfo _error = new HandleErrorInfo(ex, "Articles", "AddComment");
                return View("Error", _error);
            }
        }
       
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Follow(Guid articleId)
        {
            try
            {
                var _object = _articleManager.GetById(articleId);
                if(!_object.Followers.Contains(User.Identity.Name))
                {
                    _object.Followers = _object.Followers + User.Identity.Name + ";";
                    _object.LastUpdatedBy = User.Identity.Name;
                    _articleManager.Update(_object);
                }
                
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Followers", _object.Followers);
                }
                else
                {
                    return RedirectToAction("Details", "Articles", new { id = articleId });
                }
            }
            catch
            {
                return View();
            }
        }

        public SelectList GetSelectListCategories(ArticleModel article)
        {
            List<SelectListItem> _categoryList = new List<SelectListItem>();
            var _categories = _unitOfWorkAsync.Repository<ArticleCategory>().Query().Select().OrderBy(m => m.Name);

            ArticleCategory _default = new ArticleCategory();

            if (article.CategoryId != null)
            {
                _default = _articleCategoryManager.GetById(article.CategoryId.Value);
            }

            foreach (var item in _categories)
            {
                _categoryList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (item == _default ? true : false)
                });
            }

            return new SelectList(_categoryList, "Value", "Text");
        }

        [Authorize]
        public ActionResult Create()
        {
            ArticleModel _article = new ArticleModel();
            ViewBag.Categories = GetSelectListCategories(_article);

            return View(_article);
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleModel article)
        {
            ViewBag.Categories = GetSelectListCategories(article);

            try
            {
                article.CreatedBy = User.Identity.Name;
                if (_articleManager.Insert(article))
                {
                    var _urls = article.Content.GetYouTubeUrls();
                    if(_urls != null && _urls.Length > 0)
                    {
                        foreach(string _url in _urls)
                        {
                            var _model = new UrlModel(_url);
                            _urlManager.Insert(_model);
                        }
                    }
                }
                return View(article);
            }
            catch
            {
                return View(article);
            }
        }
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _model = _articleManager.GetById(id);
            if (_model != null)
            {
                var _categories = _unitOfWorkAsync.Repository<ArticleCategory>().Query().Select();
                List<SelectListItem> _categoryList = new List<SelectListItem>();

                ArticleCategory _default = new ArticleCategory();

                if (_model.CategoryId.HasValue)
                {
                    _default = _articleCategoryManager.GetById(_model.CategoryId.Value);
                }

                foreach (var item in _categories)
                {
                    _categoryList.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                        Selected = (item == _default ? true : false)
                    });
                }

                var selectList = new SelectList(_categoryList, "Value", "Text");

                ViewBag.Categories = selectList;

                var _view = _mapper.ToView(_model);
                return View(_view);
            }
            else
            {
                return HttpNotFound();
            }
        }

        
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]                
        
        public ActionResult Edit(Guid id, ArticleView view, FormCollection collection)
        {
            try
            {
                ArticleModel _article = _mapper.ToModel(view);
                _article.LastUpdatedDate = DateTime.Now;
                _article.Status = PublishStatus.IsDrafted;
                _article.LastUpdatedBy = User.Identity.Name;
                if (_articleManager.Update(_article))
                {
                    return RedirectToAction("Details", new { id = _article.Id });
                }
                return View("_Error");
            }
            catch(Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ToPublish(Guid id)
        {
            AjaxView = "Messages/_Published";
            var _model = _articleManager.GetById(id);
            _model.Status = PublishStatus.IsPublished;
            _articleManager.Update(_model);
            if (Request.IsAjaxRequest())
            {
                return PartialView(AjaxView);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult ToPublic(Guid id)
        {
            try
            {
                var _model = _articleManager.GetById(id);
                _model.Status = PublishStatus.IsPublished;
                _articleManager.Update(_model);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Public");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ToDraft(Guid id)
        {
            try
            {
                var _model = _articleManager.GetById(id);
                _model.Status = PublishStatus.IsDrafted;
                _model.LastUpdatedBy = User.Identity.Name;
                _articleManager.Update(_model);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Drafted");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _model = _articleManager.GetById(id);
            if(_model != null)
            {
                return View(_model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ArticleModel model)
        {
            if (_articleManager.Delete(model))
            {
                return RedirectToAction("Index", "Articles");
            }
            return View();
        }
    }
}
