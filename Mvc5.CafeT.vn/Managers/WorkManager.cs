using CafeT.BusinessObjects;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class WorkManager : ObjectManager
    {
        public WorkManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public WorkModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<WorkModel>().Find(id);
            return _object;
        }

        public CompanyModel GetCompany(Guid companyId)
        {
            return _unitOfWorkAsync.Repository<CompanyModel>().Find(companyId);
        }

        public void Update(WorkModel model)
        {
            _unitOfWorkAsync.Repository<WorkModel>().Update(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Insert(WorkModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<WorkModel>().Insert(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IEnumerable<WorkModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<WorkModel>().Query()
                            .Select();

            return _models.AsEnumerable();
        }
    }
}