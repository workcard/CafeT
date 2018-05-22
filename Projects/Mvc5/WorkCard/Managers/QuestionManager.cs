using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.Managers
{
    public class QuestionManager : BaseManager
    {
        public QuestionManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }
       
        public Question GetById(Guid id)
        {
            return _unitOfWorkAsync.RepositoryAsync<Question>()
                .Find(id);
        }

        public List<Question> GetAll()
        {
            return _unitOfWorkAsync.RepositoryAsync<Question>()
                .Queryable()
                .OrderByDescending(t => t.CreatedDate)
                .ToList();
        }

        public List<Question> GetAllOf(string userName)
        {
            return _unitOfWorkAsync.RepositoryAsync<Question>()
                .Queryable()
                .Where(t => (t.CreatedBy.ToLower() == userName))
                .OrderByDescending(t=>t.CreatedDate)
                .ToList();
        }
        
        public bool Insert(Question issue)
        {
            _unitOfWorkAsync.Repository<Question>().Insert(issue);
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result < 0) return false;
            return true;
        }
        public bool Update(Question issue)
        {
            _unitOfWorkAsync.Repository<Question>().Update(issue);
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result < 0) return false;
            return true;
        }

        public bool Delete(Guid id)
        {
            var workIssue = GetById(id);
            _unitOfWorkAsync.Repository<Question>().Delete(workIssue);
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result < 0) return false;
            return true;
        }

        public IEnumerable<Answer> GetAnswers(Guid id)
        {
            return _unitOfWorkAsync.RepositoryAsync<Answer>()
                .Queryable()
                .Where(t => t.QuestionId == id)
                .OrderByDescending(t => t.CreatedDate)
                .ToList();
        }

        public bool HasAnswers(Guid id)
        {
            var _answers = GetAnswers(id);
            if (_answers != null && _answers.Count() > 0) return true;
            return false;
        }
    }
}