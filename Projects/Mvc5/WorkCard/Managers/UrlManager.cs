using Repository.Pattern.UnitOfWork;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Managers
{
    public class UrlManager:BaseManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public UrlManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }
        public Url GetById(Guid id)
        {
            return _unitOfWorkAsync.RepositoryAsync<Url>().Find(id);
        }
        public void Update(Url url)
        {
            _unitOfWorkAsync.RepositoryAsync<Url>().Update(url);
            _unitOfWorkAsync.SaveChangesAsync();
        }
        public async Task<bool> AddAsync(Url url)
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