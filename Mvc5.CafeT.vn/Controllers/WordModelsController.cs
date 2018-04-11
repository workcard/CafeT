using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;

namespace Mvc5.CafeT.vn.Controllers
{
    public class WordModelsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public WordModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public async Task<ActionResult> Index()
        {
            var models = _unitOfWorkAsync.RepositoryAsync<WordModel>()
                .Query().Select();
            return View(models);
        }

        [Authorize]
        public async Task<ActionResult> TopWords()
        {
            var models = _unitOfWorkAsync.RepositoryAsync<WordModel>()
                        .Query().Select()
                        .Where(t => t.IsOf(User.Identity.Name))
                        .Where(t => !t.IsRemembered);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Words/_WordModels", models);
            }
            else
            {
                return View("Words/_WordModels", models);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Save(string model)
        {
            WordModel _model = new WordModel(model);
            _model.CreatedBy = User.Identity.Name;
            bool _inserted = _wordManager.Insert(_model);

            if(_inserted)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Messages/_HtmlString", "Saved");
                }
                else
                {
                    return View("Messages/_HtmlString", "Saved");
                }
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Messages/_HtmlString", "Can't insert");
                }
                else
                {
                    return View("Messages/_HtmlString", "Can't insert");
                }
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult Remembered(Guid id)
        {
            var model = _unitOfWorkAsync.RepositoryAsync<WordModel>().Find(id);
            model.IsRemembered = true;
            
            _unitOfWorkAsync.RepositoryAsync<WordModel>().Update(model);
            try
            {
                int _inserted = _unitOfWorkAsync.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Messages/_HtmlString", "Saved");
                }
                else
                {
                    return View("Messages/_HtmlString", "Saved");
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Messages/_HtmlString", ex.Message);
                }
                else
                {
                    return View("Messages/_HtmlString", ex.Message);
                }
            }
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordModel wordModel = await db.Words.FindAsync(id);
            
            if (wordModel == null)
            {
                return HttpNotFound();
            }
            return View(wordModel);
        }
        public async Task<ActionResult> GetWordInfo(Guid id)
        {
            WordModel wordModel = await db.Words.FindAsync(id);
            if(Request.IsAjaxRequest())
            {
                return PartialView("");
            }
            else
            {
                return View("");
            }
        }
        // GET: WordModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WordModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WordModel wordModel)
        {
            if (ModelState.IsValid)
            {
                wordModel.Id = Guid.NewGuid();
                db.Words.Add(wordModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(wordModel);
        }

        // GET: WordModels/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordModel wordModel = await db.Words.FindAsync(id);
            if (wordModel == null)
            {
                return HttpNotFound();
            }
            return View(wordModel);
        }

        // POST: WordModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WordModel wordModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wordModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(wordModel);
        }

        // GET: WordModels/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordModel wordModel = await db.Words.FindAsync(id);
            if (wordModel == null)
            {
                return HttpNotFound();
            }
            return View(wordModel);
        }

        // POST: WordModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WordModel wordModel = await db.Words.FindAsync(id);
            db.Words.Remove(wordModel);
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
