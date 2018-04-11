using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Web.Managers;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class ContactsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ContactsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public async Task<ActionResult> Index()
        {
            var _myContacts = db.Contacts.Where(t => t.UserName == User.Identity.Name).AsEnumerable();
            if(User.Identity.IsAuthenticated)
            {
                return View("Index", _myContacts);
            }
            return View(db.Contacts.AsEnumerable());
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetContacts()
        {
            var _objects = ContactManager.GetContacts(User.Identity.Name);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Contacts", _objects);
            }
            return View(_objects);
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            var _issues = ContactManager.GetIssuesOf(contact.Email).ToList();
            ViewBag.Issues = Mappers.IssueMappers.IssuesToViews(_issues);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }
        public async Task<ActionResult> GetContact(string email)
        {
            Contact contact = ContactManager.GetByEmail(email);
            var _issues = ContactManager.GetIssuesOf(contact.Email).ToList();
            ViewBag.Issues = Mappers.IssueMappers.IssuesToViews(_issues);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View("Details",contact);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddContact(string email)
        {
            Contact _contact = new Contact(email);
            _contact.CreatedBy = User.Identity.Name;
            _contact.UserName = User.Identity.Name;

            bool _isAdded = await ContactManager.AddContactAsync(_contact);
            if(_isAdded)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_WorkTime", "Đã thêm");
                }
                return RedirectToAction("Index");
            }
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_WorkTime", "Không thêm được. Thành viên này đã có");
                }
                return RedirectToAction("Index");
            }
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Id = Guid.NewGuid();
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

    
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            db.Contacts.Remove(contact);
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
