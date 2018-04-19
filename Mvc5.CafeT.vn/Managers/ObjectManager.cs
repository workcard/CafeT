using Repository.Pattern.UnitOfWork;
//using Service.Pattern;

namespace Mvc5.CafeT.vn.Managers
{
    public class ObjectManager
    {
        protected IUnitOfWorkAsync _unitOfWorkAsync;
        //protected IService<> _service;
        public string ErrorMessage { set; get; }

        public ObjectManager(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            ErrorMessage = "Developer is not good. Pls try and catch exception.";
        }
        //public void Delete<TEntity>(Guid id)
        //{
        //    _unitOfWorkAsync.RepositoryAsync<T>().Delete(id);
        //}
    }
}