﻿using CafeT.Text;
using MoreLinq;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
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

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Find(string keyword)
        {
            if(keyword.Contains("{"))
            {
                keyword = keyword.GetInBetween("{", "}", false, false);
            }
            var contacts = db.Contacts.AsEnumerable();
            List<Contact> list = new List<Contact>();
            foreach(var contact in contacts)
            {
                if(contact.Contains(keyword))
                {
                    list.Add(contact);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var _myContacts = db.Contacts.Where(t => t.CreatedBy == User.Identity.Name)
                .OrderByDescending(t=>t.UpdatedDate)
                .ThenByDescending(t=>t.CreatedDate)
                .AsEnumerable();

            if(User.Identity.IsAuthenticated)
            {
                return View("Index", _myContacts);
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetContacts()
        {
            var _objects = ContactManager.GetContacts(User.Identity.Name);
            _objects = _objects.DistinctBy(t=>t.Email).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Contacts", _objects);
            }
            return View(_objects);
        }

        [Authorize]
        public async Task<ActionResult> Details(Guid? id)
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

        //[HttpPost]
        //[Authorize]
        //public async Task<ActionResult> AddContact(string email)
        //{
        //    Contact _contact = new Contact(email);
        //    _contact.CreatedBy = User.Identity.Name;
            

        //    bool _isAdded = await ContactManager.AddContactAsync(_contact,User.Identity.Name);
        //    if(_isAdded)
        //    {
        //        if (Request.IsAjaxRequest())
        //        {
        //            return PartialView("Issues/_WorkTime", "Đã thêm");
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    {
        //        if (Request.IsAjaxRequest())
        //        {
        //            return PartialView("Issues/_WorkTime", "Không thêm được. Thành viên này đã có");
        //        }
        //        return RedirectToAction("Index");
        //    }
        //}

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        
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
