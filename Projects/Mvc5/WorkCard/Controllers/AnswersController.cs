using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Web.Models;

namespace Web.Controllers
{
    public class AnswersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public AnswersController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: Answers
        public async Task<ActionResult> Index()
        {
            return View(await db.Answers.ToListAsync());
        }

        // GET: Answers/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = await db.Answers.FindAsync(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

       
        [Authorize]
        public ActionResult Create(Guid questionId)
        {
            Answer answer = new Answer();
            answer.QuestionId = questionId;
            if(Request.IsAjaxRequest())
            {
                return PartialView("Answers/_AddAnswerAjax", answer);
            }
            return View(answer);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        [PreventDuplicateRequest]
        public async Task<ActionResult> Create(Answer answer)
        {
            if (ModelState.IsValid)
            {
                answer.CreatedBy = User.Identity.Name;
                db.Answers.Add(answer);
                await db.SaveChangesAsync();
                if(Request.IsAjaxRequest())
                {
                    return PartialView("Answers/_AnswerItem", answer);
                }
                return RedirectToAction("Details","Questions", new { id = answer.QuestionId});
            }

            return View(answer);
        }

        // GET: Answers/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = await db.Answers.FindAsync(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            if(Request.IsAjaxRequest())
            {
                return PartialView("Answers/_Edit", answer);
            }
            return View(answer);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(answer);
        }

        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = await db.Answers.FindAsync(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Answer answer = await db.Answers.FindAsync(id);
            db.Answers.Remove(answer);
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
