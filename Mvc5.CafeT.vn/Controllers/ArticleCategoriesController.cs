﻿using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using PagedList;

namespace Mvc5.CafeT.vn.Controllers
{
    public class ArticleCategoriesController : BaseController
    {

        public ArticleCategoriesController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public async Task<ActionResult> Index()
        {
            var _objects = _unitOfWorkAsync.Repository<ArticleCategory>().Query().Select().OrderBy(t => t.Name);
            if(Request.IsAjaxRequest())
            {
                return PartialView("Articles/_Categories", _objects);
            }
            return View(_objects);
        }

        [HttpGet]
        public ActionResult GetArticles(Guid categoryId, int? page)
        {
            var _objects = _articleCategoryManager.GetArticles(categoryId);
            var _articleViews = _mapper.ToViews(_objects.ToList());
            if (Request.IsAjaxRequest())
            {
                return PartialView("Articles/_Articles", _articleViews.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
            }
            return View("Articles/_Articles", _articleViews.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
        }

        public async Task<ActionResult> GetArticles(Guid categoryId, int? page, string searchString)
        {
            var _objects = _articleCategoryManager.GetArticles(categoryId);
            var _articleViews = _mapper.ToViews(_objects.ToList());
            if (Request.IsAjaxRequest())
            {
                return PartialView("Articles/_Articles", _articleViews.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
            }
            return View("Articles/_Articles", _articleViews.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
        }

        
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var articleCategory = _articleCategoryManager.GetById(id.Value);
            if (articleCategory == null)
            {
                return HttpNotFound();
            }
            return View(articleCategory);
        }

        // GET: ArticleCategories/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public async Task<ActionResult> Create(ArticleCategory articleCategory)
        {
            if (ModelState.IsValid)
            {
                articleCategory.Id = Guid.NewGuid();
                articleCategory.CreatedBy = User.Identity.Name;
                if(_articleCategoryManager.Insert(articleCategory))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(articleCategory);
        }

        // GET: ArticleCategories/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var articleCategory = _articleCategoryManager.GetById(id.Value);
            if (articleCategory == null)
            {
                return HttpNotFound();
            }
            return View(articleCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public async Task<ActionResult> Edit(ArticleCategory articleCategory)
        {
            if (ModelState.IsValid)
            {
                if(_articleCategoryManager.Update(articleCategory))
                {
                    return RedirectToAction("Index");
                } 
            }
            return View(articleCategory);
        }

        // GET: ArticleCategories/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var articleCategory = _articleCategoryManager.GetById(id.Value);
            if (articleCategory == null)
            {
                return HttpNotFound();
            }
            return View(articleCategory);
        }

        // POST: ArticleCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(ArticleCategory model)
        {
            if (_articleCategoryManager.Delete(model))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
