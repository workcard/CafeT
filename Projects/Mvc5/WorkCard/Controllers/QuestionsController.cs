using CafeT.Enumerable;
using CafeT.GoogleManager;
using CafeT.Text;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
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
        //private ApplicationDbContext db = new ApplicationDbContext();
       
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
            var _questions = _unitOfWorkAsync.RepositoryAsync<Question>()
                .Query().Select().OrderByDescending(t => t.CreatedDate.Value)
                .AsEnumerable();

            return View(_questions.ToList());
        }

        [HttpGet]
        public ActionResult GetLastQuestions(int? n)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<Question>()
                .Query().Select()
                .OrderByDescending(t=>t.CreatedDate)
                .ThenByDescending(t=>t.UpdatedDate)
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
            var question = QuestionManager.GetById(id.Value);
            
            if (question == null)
            {
                return HttpNotFound();
            }
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
                return PartialView("Questions/_AddQuestionAjax", question);
            }
            return View(question);
        }
        
        public ActionResult Translate(Guid id)
        {
            Question question = QuestionManager.GetById(id);
            string _dest = Translator.Translate(question.Content, "en");
            if (Request.IsAjaxRequest())
            {
                return PartialView("_NotifyMessage", _dest);
            }
            return View("_NotifyMessage", _dest);
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
                var insterted = QuestionManager.Insert(question);
                if(insterted)
                {
                    question.Notify(EmailService);
                    await ProcessQuestion(question);

                }

                if (Request.IsAjaxRequest())
                {
                    return PartialView("Questions/_QuestionItem", question);
                }
                return RedirectToAction("Index");
            }

            return View(question);
        }

        private async Task ProcessQuestion(Question question)
        {
            if (question.IssueId == null) return;
            var emails = question.Content.GetEmails();
            if (emails == null) return;

            var issueContacts = ContactManager.GetContactsOfIssue(question.IssueId.Value);
            var issue = IssueManager.GetById(question.IssueId.Value);
            var nowEmails = new List<string>();
            if (issueContacts != null && issueContacts.Any())
            {
                nowEmails = issueContacts.Select(t => t.Email).ToList();
            }
            foreach (var email in emails)
            {
                if (!nowEmails.Contains(email))
                {
                    Contact contact = ContactManager.GetByEmail(email);
                    if(contact == null)
                    {
                        contact = new Contact(email) { CreatedBy = User.Identity.Name };
                        ContactManager.AddContact(contact);
                    }
                    ContactManager.MappContactIssue(contact, issue);
                }
                else
                {
                    var contact = ContactManager.GetByEmail(email);
                    contact.Issues.Add(issue);
                    issue.Contacts.Add(contact);
                    await IssueManager.UpdateAsync(issue);
                    ContactManager.MappContactIssue(contact, issue);
                }
            }
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
                _unitOfWorkAsync.RepositoryAsync<Question>().Insert(question);
                await _unitOfWorkAsync.SaveChangesAsync();

                if(question.IssueId.HasValue)
                {
                    return RedirectToAction("Details", "WorkIssues",  new { id = question.Id});
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
            var question = QuestionManager.GetById(id.Value);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                QuestionManager.Update(question);
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
            Question question = QuestionManager.GetById(id.Value);
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
            QuestionManager.Delete(id);
            if(Request.IsAjaxRequest())
            {
                return PartialView("_DeleteMsg");
            }
            return RedirectToAction("Index");
        }
    }
}
