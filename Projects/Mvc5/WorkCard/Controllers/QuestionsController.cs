﻿using CafeT.Enumerable;
using Repository.Pattern.UnitOfWork;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{

    public class QuestionsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string SubscriptionKey = "3c45b3b12bda4d568939ab4fc7245944";
        //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com
        public QuestionsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        [HttpGet]
        public ActionResult GetLastAnswers(string id)
        {
            var Id = Guid.Parse(id);
            var _objects = QuestionManager.GetAnswers(Id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Answers/_Answers", _objects);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Index()
        {
            return View(await db.Questions
                .OrderByDescending(t=>t.CreatedDate.Value)
                .ToListAsync());
        }

        [HttpGet]
        public ActionResult GetLastQuestions(int? n)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<Question>()
                .Query().Select()
                .OrderByDescending(t => t.UpdatedDate)
                .TakeMax(n)
                .ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Questions/_Questions", _objects);
            }
            return RedirectToAction("Index");
        }

      
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = await db.Questions.FindAsync(id);
            
            if (question == null)
            {
                return HttpNotFound();
            }
            question.Answers = await db.Answers
                    .Where(t => t.QuestionId == id.Value)
                    .ToListAsync();
            return View(question);
        }

        [Authorize]
        public ActionResult Create()
        {
            Question question = new Question();
            return View(question);
        }

        [Authorize]
        public ActionResult AddQuestion(Guid issueId)
        {
            Question question = new Question();
            question.IssueId = issueId;
            if(Request.IsAjaxRequest())
            {
                return PartialView("Issues/_AddQuestionAjax", question);
            }
            return View(question);
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public async Task<ActionResult> AjaxCreate(Question question)
        {
            if (ModelState.IsValid)
            {
                question.CreatedBy = User.Identity.Name;
                db.Questions.Add(question);

                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return PartialView("Questions/_QuestionItem", question);
                }
                return RedirectToAction("Index");
            }

            return View(question);
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Question question)
        {
            if (ModelState.IsValid)
            {
                question.CreatedBy = User.Identity.Name;
                db.Questions.Add(question);

                await db.SaveChangesAsync();

                if(question.IssueId.HasValue)
                {
                    return RedirectToAction("Details", "WorkIssues",  
                        new { id = question.IssueId.Value });
                }
                return RedirectToAction("Index");
            }

            return View(question);
        }

        // GET: Questions/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = await db.Questions.FindAsync(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = await db.Questions.FindAsync(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Question question = await db.Questions.FindAsync(id);
            db.Questions.Remove(question);
            await db.SaveChangesAsync();
            if(Request.IsAjaxRequest())
            {
                return PartialView("_DeleteMsg");
            }
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