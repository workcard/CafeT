using CafeT.Enumerable;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public async Task<ActionResult> Index()
        {
            return View(await db.Articles.ToListAsync());
        }
        public async Task<ActionResult> GetLastestArticles(int? n)
        {
            var articles = await db.Articles.ToListAsync();
            articles = articles.OrderByDescending(t => t.CreatedDate).ToList();
            if(Request.IsAjaxRequest())
            {
                return PartialView("_Articles", articles.TakeMax(n.Value));
            }
            return View("Index", articles.TakeMax(n.Value));
        }

        [HttpGet]
        public async Task<ActionResult> GetDocuments(Guid articleId)
        {
            var _objects = await db.Documents
                .Where(t=>t.ArticleId.HasValue && t.ArticleId.Value == articleId)
                .ToListAsync();
            var _views = _objects;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Files", _views);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Content,ProjectId,IssueId,QuestionId,JobId,CreatedDate,UpdatedDate,UpdatedBy,CreatedBy")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.Id = Guid.NewGuid();
                article.CreatedBy = User.Identity.Name;
                article.CreatedDate = DateTime.Now;
                db.Articles.Add(article);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                article.UpdatedBy = User.Identity.Name;
                article.UpdatedDate = DateTime.Now;
                db.Entry(article).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Article article = await db.Articles.FindAsync(id);
            db.Articles.Remove(article);
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
