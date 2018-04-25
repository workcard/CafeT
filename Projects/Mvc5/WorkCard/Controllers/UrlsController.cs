using CafeT.Html;
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
    public class UrlsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<ActionResult> Index()
        {
            var _objects = db.Urls.AsEnumerable();
            if(User.Identity.IsAuthenticated)
            {
                _objects = _objects.Where(t => t.CreatedBy == User.Identity.Name).AsEnumerable();
            }
            return View(_objects);
        }

       
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Url url = await db.Urls.FindAsync(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            //var page = new WebPage(url.Address);
            //page.Load();
            url.Load();
            ViewBag.Title = url.Title;
            return View(url);
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<ActionResult> AddUrl(string url)
        //{
        //    UrlManager _manager = new UrlManager();
        //    Url _url = new Url(url);
        //    _url.CreatedBy = User.Identity.Name;

        //    bool _isAdded = await _manager.AddAsync(_url);

        //    if (_isAdded)
        //    {
        //        if (Request.IsAjaxRequest())
        //        {
        //            return PartialView("_WorkTime", "Đã thêm");
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    {
        //        if (Request.IsAjaxRequest())
        //        {
        //            return PartialView("_WorkTime", "Không thêm được. Địa chỉ này đã có");
        //        }
        //        return RedirectToAction("Index");
        //    }
        //}

        // GET: Urls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Urls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(Url url)
        {
            if (ModelState.IsValid)
            {
                url.Id = Guid.NewGuid();
                db.Urls.Add(url);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(url);
        }

        // GET: Urls/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Url url = await db.Urls.FindAsync(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            return View(url);
        }

        // POST: Urls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Url url)
        {
            if (ModelState.IsValid)
            {
                db.Entry(url).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(url);
        }

        // GET: Urls/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Url url = await db.Urls.FindAsync(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            return View(url);
        }

        // POST: Urls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Url url = await db.Urls.FindAsync(id);
            db.Urls.Remove(url);
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
