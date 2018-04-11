using CafeT.Time;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using CafeT.Text;
using CafeT.MathText;
using CafeT.Enumerable;

namespace Mvc5.CafeT.vn.Managers
{
    public class QuestionManager : ObjectManager
    {
        public QuestionManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public QuestionModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<QuestionModel>().Find(id);
            _object.Answers = GetAnswers(id).ToList();
            return _object;
        }

        public bool Update(QuestionModel model)
        {
            _unitOfWorkAsync.Repository<QuestionModel>().Update(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Insert(QuestionModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Insert(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool Delete(QuestionModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Delete(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool Delete(Guid id)
        {
            _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Delete(id);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public IEnumerable<QuestionModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select().OrderByDescending(t => t.CreatedDate);
            return _models.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetAllByArticle(Guid id)
        {
            var _comments = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(c => c.ArticleId != null && c.ArticleId.HasValue && c.ArticleId.Value == id);

            return _comments.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetAllByLevel(int level)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(t=>t.Level == level)
                            .OrderByDescending(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetAllVerified()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(t => t.IsVerified)
                            .OrderByDescending(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetAllCreateBy(string userName)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(t=>t.CreatedBy == userName)
                            .OrderByDescending(t => t.CreatedDate); 
            return _models.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetAllVerifiedBy(string userName)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(t => t.VerifiedBy == userName)
                            .OrderByDescending(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }

        //public IEnumerable<QuestionModel> GetAllNoAnswers()
        //{
        //    var _all = GetAll();
        //    List<QuestionModel> _models = new List<QuestionModel>();
        //    foreach (var _item in _all)
        //    {
        //        _item.Answers = GetAnswers(_item.Id);
        //        if(_item.Answers == null || _item.Answers.Count() == 0)
        //        {
        //            _models.Add(_item);
        //        }
                
        //    }
        //    return _models.Where(t => t.Answers != null).OrderByDescending(t => t.CreatedDate); ;
        //}

        public IEnumerable<QuestionModel> GetNews(int days, int ?n)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                .Select()
                .Where(t=>t.IsVerified && t.IsNew(days))
                .OrderByDescending(t => t.CreatedDate)
                .TakeMax(n);
            return _models;
        }

        public IEnumerable<QuestionModel> GetLastest(int? n)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                .Select()
                .Where(t => t.IsVerified)
                .OrderByDescending(t => t.CreatedDate)
                .TakeMax(n);
            return _models;
        }

        public IEnumerable<QuestionModel> GetLastestUnVerify(int? n)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                .Select()
                .Where(t => !t.IsVerified)
                .OrderByDescending(t => t.CreatedDate)
                .TakeMax(n);
            return _models;
        }

        public IEnumerable<QuestionModel> GetTopAnswers()
        {
            var _all = GetAll();
            List<QuestionModel> _models = new List<QuestionModel>();
            foreach(var _item in _all)
            {
                _item.Answers = GetAnswers(_item.Id);
                _models.Add(_item);
            }
            return _models.Where(t => t.Answers != null).OrderByDescending(t => t.Answers.Count()); ;
        }

        public IEnumerable<AnswerModel> GetAnswers(Guid questionId)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<AnswerModel>()
                .Query()
                .Select()
                .Where(t=>t.IsOfQuestion(questionId))
                .OrderByDescending(t => t.CreatedDate);
            return _models.AsEnumerable();
        }
    }
}