using CafeT.Enumerable;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.ModelViews;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class CommentsController : BaseController
    {
        public CommentsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        [AllowAnonymous]
        public ActionResult TopComments()
        {
            var _objects = _commentManager.GetAll().OrderByDescending(m => m.CreatedDate).TakeMax(3);
            var _views = _mapper.ToViews(_objects.ToList());
            if (Request.IsAjaxRequest())
            {
                return PartialView("Comments/_Items", _views);
            }
            else
            {
                return View("Comments/_Items", _views);
            }
        }

        [AllowAnonymous]
        public ActionResult GetCommentsByArticle(Guid articleId)
        {
            var _objects = _commentManager.GetAllByArticle(articleId);
            var _views = _mapper.ToViews(_objects.ToList());
            if (Request.IsAjaxRequest())
            {
                return PartialView("Comments/_CommentsOfArticle", _views);
            }
            else
            {
                return View("Comments/_CommentsOfArticle", _views);
            }
        }
        [AllowAnonymous]
        public ActionResult GetLastComment(Guid articleId)
        {
            var _object = _commentManager.GetAllByArticle(articleId).LastOrDefault();
            var _view = _mapper.ToView(_object);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Comments/_LastComment", _view);
            }
            else
            {
                return View("Comments/_LastComment", _view);
            }
        }
        // GET: Articles
        public ActionResult Index()
        {
            IEnumerable<CommentModel> _objects = _commentManager.GetAll();
            IEnumerable<CommentView> _views = _mapper.ToViews(_objects.ToList());
            return View(_views);
        }

        // GET: Articles/Details/5
        public ActionResult Details(Guid id)
        {
            var _comment = _commentManager.GetById(id);
            return View(_comment);
        }

        public ActionResult Create()
        {
            CommentModel _article = new CommentModel();
            return View(_article);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CommentModel comment)
        {
            try
            {
                if(_commentManager.Insert(comment))
                {
                    return RedirectToAction("Index");
                }
                return View("Errors");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _comment = _commentManager.GetById(id);
            return View(_comment);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CommentModel model)
        {
            try
            {
                model.LastUpdatedDate = DateTime.Now;
                model.LastUpdatedBy = User.Identity.Name;
                if(_commentManager.Update(model))
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Articles/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _comment = _commentManager.GetById(id);
            return View(_comment);
        }

        // POST: Articles/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(CommentModel model)
        {
            try
            {
                if (_commentManager.Delete(model))
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
