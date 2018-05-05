using CafeT.Enumerable;
using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using PagedList;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class QuestionsController : BaseController
    {
        public QuestionsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public ActionResult Index(int? page, string searchString)
        {
            var _objects = _questionManager.GetAll();
            List<QuestionModel> _views = new List<QuestionModel>();
            _views = _objects.ToList();
            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _views = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Content != null && s.Content.ToLower().Contains(searchString.ToLower())))
                            .ToList();
            }

            ViewBag.NoAnswers = _objects.Where(t => !t.HasAnswer()).AsEnumerable();
            ViewBag.TopAnswers = _questionManager.GetTopAnswers().TakeMax(5);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Questions/_Questions", _views.ToPagedList(pageNumber: page ?? 1, pageSize: 10));
            }
            return View(_views.ToPagedList(pageNumber: page ?? 1, pageSize: 10));
        }

        public ActionResult GetAllByLevel(int level)
        {
            var _objects = _questionManager.GetAllByLevel(level);
            if(Request.IsAjaxRequest())
            {
                return PartialView("Questions/_Questions", _objects);
            }
            return View("Questions/_Questions", _objects);
        }

        public ActionResult GetNews(int n)
        {
            var _objects = _questionManager.GetLastest(n);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Questions/_QuestionsForHome", _objects);
            }
            return View("Questions/_QuestionsForHome", _objects);
        }

        public ActionResult GetLastestUnVerify(int n)
        {
            var _objects = _unitOfWorkAsync.Repository<QuestionModel>().Query().Select()
                .Where(t => !t.IsVerified)
                .TakeMax(n);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Questions/_Questions", _objects);
            }
            return View("Questions/_Questions", _objects);
        }

        public ActionResult Details(Guid id)
        {
            var _object = _questionManager.GetById(id);
           
            if (_object != null && _object.EstimationTime != null)
            {
                _object.Answers = _questionManager.GetAnswers(id).ToList();
                ViewBag.AnswerCreate = new AnswerModel() { QuestionId = id };

                //if(_object.JobId != null && _object.JobId.HasValue)
                //{
                //    ViewBag.Job = _jobManager.GetById(_object.JobId.Value);
                //}
                
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Verified(Guid id)
        {
            var _model = _questionManager.GetById(id);
            _model.IsVerified = true;
            bool _result = _questionManager.Update(_model);
            if(_result)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Messages/_Verified");
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Messages/_Message","Can't verified this question");
            }
        }
       
        [HttpGet]
        [Authorize]
        public ActionResult GetAnswers(Guid questionId)
        {
            var _answers = _questionManager.GetAnswers(questionId);
            if (_answers != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Answers/_Answers", _answers);
                }
                return View("Answers/_Answers", _answers);
            }
            return HttpNotFound();
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddAnswer(Guid id)
        {
            var _question = _questionManager.GetById(id);
            if(_question != null)
            {
                if (_question.CreatedBy != User.Identity.Name)
                {
                    if (_question.IsVerified)
                    {
                        AnswerModel _answer = new AnswerModel();
                        _answer.CreatedBy = User.Identity.Name;
                        _answer.QuestionId = id;
                        ViewBag.Question = _question;

                        if (Request.IsAjaxRequest())
                        {
                            return PartialView("Questions/_UserCreateAnswer", _answer);
                        }
                        return View("Questions/_UserCreateAnswer", _answer);
                    }
                    else
                    {
                        return View("Messages/_Message", "Question is not verified. Can't add answer");
                    }
                }
                else
                {
                    AnswerModel _answer = new AnswerModel();
                    _answer.CreatedBy = User.Identity.Name;
                    _answer.QuestionId = id;

                    ViewBag.Question = _question;

                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("Questions/_UserCreateAnswer", _answer);
                    }
                    return View("Questions/_UserCreateAnswer", _answer);
                }
            }
            else
            {
                return View("Messages/_Message", "Question is null. Can't add answer");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UserCreateAnswer(AnswerModel model)
        {
            model.Id = Guid.NewGuid();
            _unitOfWorkAsync.Repository<AnswerModel>().Insert(model);
            _unitOfWorkAsync.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                var _objects = _questionManager.GetAnswers(model.QuestionId.Value);
                return PartialView("Answers/_Answers", _objects);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AdminCreateAnswer(AnswerModel model)
        {
            try
            {
                model.CreatedBy = User.Identity.Name;
                _unitOfWorkAsync.Repository<AnswerModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                if(Request.IsAjaxRequest())
                {
                    var _objects = _questionManager.GetAnswers(model.Id);
                    return PartialView("Answers/_Answers", _objects);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [Authorize]
        public ActionResult Create()
        {
            QuestionModel _object = new QuestionModel();
            return View(_object);
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionModel question)
        {
            try
            {
                question.CreatedBy = User.Identity.Name;
                if(_questionManager.Insert(question))
                {
                    return RedirectToAction("Index");
                }
                return View("CanNotCreate");
            }
            catch
            {
                return View();
            }
        }

        // GET: Questions/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<QuestionModel>().Find(id);
            if (_object != null)
            {
                if (_object.CreatedBy == User.Identity.Name)
                {
                    if (!_object.Authors.IsNullOrEmptyOrWhiteSpace())
                    {
                        _object.Authors = _object.CreatedBy;
                    }
                    return View(_object);
                }
                else
                {
                    return View("Messages/_EditNotification");
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionModel edit)
        {
            // TODO: Add update logic here
            edit.UpdatedDate = DateTime.Now;
            edit.UpdatedBy = User.Identity.Name;
            _unitOfWorkAsync.Repository<QuestionModel>().Update(edit);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                if(Request.IsAjaxRequest())
                {
                    return PartialView("Messages/_UpdatedMessage");
                }
                return RedirectToAction("Details", new { id = edit.Id });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: Questions/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _object = _questionManager.GetById(id);
            if (_object != null)
            {
                if(_object.CreatedBy == User.Identity.Name)
                {
                    return View("Delete", _object);
                }
                else
                {
                    return View("_DeleteNotification");
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Questions/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(QuestionModel model)
        {
            if (_questionManager.Delete(model.Id))
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Messages/_DeletedMessage");
                }
                return RedirectToAction("Index");
            }
            return View("Messages/_HtmlString", "Can not Delete this question");
        }
    }
}
