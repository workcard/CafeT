using Repository.Pattern.UnitOfWork;
using System.Linq;
using Web.Models;

namespace Web.Managers
{
    public class UrlManager:BaseManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public UrlManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public async System.Threading.Tasks.Task<bool> AddAsync(Url url)
        {
            var _myUrls = db.Urls.Where(t => t.CreatedBy == url.CreatedBy).Select(t => t.Address);
            if (!_myUrls.Contains(url.Address))
            {
                db.Urls.Add(url);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}