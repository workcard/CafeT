using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Mvc5.CafeT.vn.Models;
using System.Linq;

namespace Mvc5.CafeT.vn.Controllers
{
    public class AnswersController : BaseController
    {
        public AnswersController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: Answers
        public ActionResult Index()
        {
            var _objects = _answerManager.GetAll();
            return View(_objects);
        }

        public ActionResult Details(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<AnswerModel>().Find(id);

            _object.Reviews = _unitOfWorkAsync.Repository<AnswerReviewModel>()
                .Query().Select().Where(t => t.AnswerId == id)
                .AsEnumerable();

            if(_object != null)
            {
                if (_object.QuestionId != null && _object.QuestionId.HasValue)
                {
                    var _question = _unitOfWorkAsync.Repository<QuestionModel>().Find(_object.QuestionId);
                    ViewBag.Question = _question;
                }
                return View(_object);
            }
            return HttpNotFound();
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetToReview(Guid id)
        {
            var model = _unitOfWorkAsync.Repository<AnswerModel>().Find(id);
            if(model != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Answers/_ToReview", model);
                }
                else
                {
                    return View("Answers/_ToReview", model);
                }
            }
            return HttpNotFound();
        }

        [Authorize]
        public ActionResult AddReview(Guid answerId)
        {
            AnswerReviewModel _object = new AnswerReviewModel();
            _object.AnswerId = answerId;
            _object.CreatedBy = User.Identity.Name;
            _object.ReviewBy = User.Identity.Name;
            _object.ReviewDate = DateTime.Now;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Answers/_AddReview", _object);
            }
            else
            {
                return View("Answers/_AddReview", _object);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddReview(AnswerReviewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.AnswerId != null && model.AnswerId.HasValue)
                {
                    _answerManager.AddReview(model.AnswerId.Value, model);
                    return RedirectToAction("Details", "Answers", new { id = model.AnswerId.Value });
                }
            }
            return View(model);
        }

        // GET: Answers/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnswerModel model)
        {
            try
            {
                model.CreatedBy = User.Identity.Name;
                _unitOfWorkAsync.Repository<AnswerModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<AnswerModel>().Find(id);
            
            if (_object != null)
            {
                return View(_object);
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
        public ActionResult Edit(AnswerModel model)
        {
            try
            {
                model.LastUpdatedDate = DateTime.Now;
                model.LastUpdatedBy = User.Identity.Name;
                if (_answerManager.Update(model))
                {
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("Answers/_Answer", model);
                    }
                    return RedirectToAction("Details", new { id = model.Id });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<AnswerModel>().Find(id);
            if (_object != null)
            {
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult Delete(AnswerModel model)
        {
            try
            {
                if(_answerManager.Delete(model))
                {
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("Messages/_DeletedMessage");
                    }
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
