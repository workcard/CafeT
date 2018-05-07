using CafeT.Enumerable;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Web.Models;

namespace Web.Managers
{
    public class JobManager:BaseManager
    {
        public JobManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public Job GetById(Guid id)
        {
            return _unitOfWorkAsync.Repository<Job>().Find(id);
        }

        public List<Job> GetAllAsync(int? n)
        {
            return _unitOfWorkAsync.RepositoryAsync<Job>()
                .Queryable()
                .TakeMax(n)
                .ToList();
        }

        public List<Job> GetHotJobs(int? n)
        {
            return _unitOfWorkAsync.RepositoryAsync<Job>()
                .Queryable()
                .OrderByDescending(c=>c.CreatedDate)
                .TakeMax(n)
                .ToList();
        }

        public List<Comment> GetCommentsOf(Guid jobId)
        {
            return _unitOfWorkAsync.RepositoryAsync<Comment>()
                .Queryable().Where(t=>t.JobId.HasValue && t.JobId.Value == jobId)
                .ToList();
        }

        public List<Job> GetAllOf(string userName,int? n)
        {
            return _unitOfWorkAsync.RepositoryAsync<Job>()
                .Queryable()
                .Where(t => t.IsOf(userName))
                .TakeMax(n)
                .ToList();
        }

        public List<Job> GetAllOf(string userName, JobStatus status, int? n)
        {
            return GetAllOf(userName, n)
                .Where(t=>t.Status == status)
                .ToList();
        }
        public bool Insert(Job model)
        {
            _unitOfWorkAsync.Repository<Job>().Insert(model);
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result < 0) return false;
            return true;
        }

        public bool Update(Job model)
        {
            _unitOfWorkAsync.Repository<Job>().Update(model);
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result < 0) return false;
            return true;
        }

        public bool Delete(Guid id)
        {
            var _model = GetById(id);
            _unitOfWorkAsync.Repository<Job>().Delete(_model);
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result < 0) return false;
            return true;
        }

        public IEnumerable<Job> GetAllExpiredOf(string userName)
        {
            var _objects = _unitOfWorkAsync.Repository<Job>().Queryable().ToList();
            _objects = _objects.Where(t => t.IsOf(userName) && t.IsExpired()).ToList();
            return _objects;
        }

        public IEnumerable<Job> GetLastest(string userName)
        {
            var _objects = _unitOfWorkAsync.Repository<Job>().Queryable().ToList();
            _objects = _objects
                .Where(t => t.IsOf(userName) && t.IsExpired())
                .OrderByDescending(t => t.CreatedDate)
                .ToList();
            return _objects;
        }
    }
}