using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class InterviewModelsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public InterviewModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: InterviewModels
        public async Task<ActionResult> Index()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<InterviewModel>()
                .Query().Select().ToList();

            return View("Index", _models);
        }
        // GET: InterviewModels
        public async Task<ActionResult> GetQuestions(Guid interviewId)
        {
            var _models = _questionManager.GetAll();
            var _questions = _models.Where(t => t.InterviewId == interviewId).ToList();
            if(Request.IsAjaxRequest())
            {
                return PartialView("Questions/_Questions", _questions);
            }
            return View("Index", _models);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddQuestion(Guid interviewId)
        {
            QuestionModel _model = new QuestionModel();
            _model.CreatedBy = User.Identity.Name;
            _model.InterviewId = interviewId;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Interviews/_AddQuestion", _model);
            }
            else
            {
                return View("Interviews/_AddQuestion", _model);
            }
        }
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(QuestionModel model)
        {
            bool _result = _questionManager.Insert(model);
            if (_result)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Questions/_Question", model);
                }
                else
                {
                    return RedirectToAction("Details", "InterviewModels", new { id = model.ArticleId });
                }
            }
            return View("Messages/_Message", "Can't insert this model : " + model.ToString());
        }
        [HttpPost]
        [Authorize]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult MarkAsArticle(Guid interviewId)
        {
            var model = db.Interviews.Find(interviewId);
            model.Questions = _questionManager.GetAll()
                .Where(t => t.InterviewId.HasValue && t.InterviewId.Value == interviewId)
                .AsEnumerable();

            if (model != null)
            {
                ArticleModel _article = new ArticleModel();
                _article.Title = model.Name;
                _article.Summary = model.Description;
                _article.Content = model.MakeArticleContent();
                _article.CreatedDate = DateTime.Now;
                _article.CreatedBy = User.Identity.Name;

                var _articleView = _mapper.ToView(_article);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Articles/_ArticleItem", _articleView);
                }
                else
                {
                    return RedirectToAction("Details", "InterviewModels", new { id = model.Id });
                }
            }
            return View("Messages/_Message", "Can't insert this model : " + model.ToString());
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterviewModel interviewModel = await db.Interviews.FindAsync(id);
            if (interviewModel == null)
            {
                return HttpNotFound();
            }
            return View(interviewModel);
        }

        // GET: InterviewModels/Create
        [Authorize]
        public ActionResult Create()
        {
            InterviewModel model = new InterviewModel();
            model.CreatedBy = User.Identity.Name;
            model.InterviewDate = DateTime.Now;
            return View(model);
        }

        // POST: InterviewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InterviewModel interviewModel)
        {
            if (ModelState.IsValid)
            {
                interviewModel.Id = Guid.NewGuid();
                db.Interviews.Add(interviewModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(interviewModel);
        }

        // GET: InterviewModels/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterviewModel interviewModel = await db.Interviews.FindAsync(id);
            if (interviewModel == null)
            {
                return HttpNotFound();
            }
            return View(interviewModel);
        }

        // POST: InterviewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,InterviewDate,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,CountViews,LastViewAt,LastViewBy")] InterviewModel interviewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interviewModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(interviewModel);
        }

        // GET: InterviewModels/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterviewModel interviewModel = await db.Interviews.FindAsync(id);
            if (interviewModel == null)
            {
                return HttpNotFound();
            }
            return View(interviewModel);
        }

        // POST: InterviewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            InterviewModel interviewModel = await db.Interviews.FindAsync(id);
            db.Interviews.Remove(interviewModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
