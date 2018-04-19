using CafeT.Html;
using CafeT.SmartObjects;
using CafeT.Text;
using HtmlAgilityPack;
using Mvc5.CafeT.vn.Models;
using PagedList;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class UrlsController : BaseController
    {
        public UrlsController(IUnitOfWorkAsync unitOfWorkAsync):base(unitOfWorkAsync)
        {
        }

        public ActionResult Index(int? page, string searchString)
        {
            List<UrlModel> _models = new List<UrlModel>();
            _models = _urlManager.GetAll().ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                _models = _models.Where(s => s.Url.Contains(searchString) 
                || (s.Name != null && s.Name.Contains(searchString)))
                .OrderByDescending(t=>t.CreatedDate).ToList();
            }

            if(Request.IsAjaxRequest())
            {
                return (PartialView( _models.ToPagedList(pageNumber: page ?? 1, pageSize: 10)));
            }
            return (View(_models.ToPagedList(pageNumber: page ?? 1, pageSize: 10)));
        }

        public bool IsSelectNode(HtmlNode node)
        {
            bool _IsMinSub = node.ChildNodes.Where(t => t.InnerText.HtmlToText().GetCountWords() > 100).Count() == 0;
            bool _IsCountWords = node.InnerText.HtmlToText().GetCountWords() > 500;
            if (_IsMinSub && _IsCountWords) return true;
            return false;
        }

        public HtmlNode GetNode(HtmlNode node)
        {
            if (IsSelectNode(node))
            {
                return node;
            }
            else
            {
                while(!IsSelectNode(node))
                {
                    var _nodes = node.ChildNodes.Where(t => t.InnerText.HtmlToText().GetCountWords() > 500);
                    if(_nodes != null && _nodes.Count()>0)
                    {
                        foreach (var _node in _nodes)
                        {
                            //node = _node;
                            node = GetNode(_node);
                            //node = _node;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                return node;
            }
            
            //if(node.ChildNodes.Count == 0)
            //{
            //    if (node.InnerText.GetCountWords() > 100) return node;
            //    return null;
            //}
            //else
            //{
            //    foreach(var _node in node.ChildNodes)
            //    {
            //        GetNode(_node);
            //    }
            //    return node;
            //}
        }
        
        public ActionResult LoadNodes(Guid id)
        {
            var _model = _urlManager.GetById(id);
            _model.LoadFull();

            HtmlDocument _document = new HtmlDocument();
            _document.LoadHtml(_model.HtmlContent);

            var _nodes = _document.GetNodes()
                .Where(t=>t.InnerText.GetCountWords()>100)
                .Where(t => !t.IsHref())
                //.Select(t=> t.ChildNodes)
                .Select(t => t.PrintToString());
            //.Where(t=>t.IsCssClass())
            //.Where(t=>t.IsDiv())
            //.Where(t => !t.InnerText.IsNullOrEmptyOrWhiteSpace() && t.InnerText.ToWords().Count() > 0)
            //.Where(t=> !t.Name.IsNullOrEmptyOrWhiteSpace())
            //.Select(t => t.Name + "|" + t.XPath);

            var _node = GetNode(_document.DocumentNode);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Messages/_Menus", _nodes);
            }
            return View("Messages/_Menus", _nodes);
        }

        public ActionResult ReadUrl(Guid id)
        {
            var _model = _urlManager.GetById(id);
            _model.LoadFull();

            HtmlProcessor processor = new HtmlProcessor(_model.HtmlContent);
            string content = processor.PlanText;
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("Messages/_HtmlString", content);
            }
            return View("Messages/_HtmlString", content);
        }
        
        // GET: Urls/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UrlModel _model = _unitOfWorkAsync.Repository<UrlModel>().Find(id);
            if (_model == null)
            {
                return HttpNotFound();
            }

            return View(_model);
        }

        // GET: Urls/Create
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            UrlModel _model = new UrlModel();
            return View(_model);
        }

        // POST: Urls/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(UrlModel model)
        {
            _unitOfWorkAsync.Repository<UrlModel>().Insert(model);
            try
            {
                // TODO: Add insert logic here
                model.LastUpdatedBy = User.Identity.Name;
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("Error", ex);
            }
        }

        // GET: Urls/Edit/5
        public ActionResult Edit(Guid id)
        {
            var _model = _unitOfWorkAsync.Repository<UrlModel>().Find(id);
            return View(_model);
        }

        // POST: Urls/Edit/5
        [HttpPost]
        public ActionResult Edit(UrlModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            model.LastUpdatedBy = User.Identity.Name;
            _unitOfWorkAsync.Repository<UrlModel>().Update(model);
            try
            {
                // TODO: Add insert logic here
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        // GET: Urls/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _model = _unitOfWorkAsync.Repository<UrlModel>().Find(id);
            if (_model == null)
            {
                return HttpNotFound();
            }
            return View(_model);
        }

        // POST: Urls/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
