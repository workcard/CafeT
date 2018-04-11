using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Managers
{
    public class ArticleCategoryManager : ObjectManager
    {
        public ArticleCategoryManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }
        
        public ArticleCategory GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ArticleCategory>().Find(id);
            return _object;
        }

        public bool Update(ArticleCategory model)
        {
            _unitOfWorkAsync.Repository<ArticleCategory>().Update(model);
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

        public bool Insert(ArticleCategory model)
        {
            _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Insert(model);
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
        public bool Delete(ArticleCategory model)
        {
            _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Delete(model);
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

        public IEnumerable<ArticleCategory> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Query()
                            .Select();
            return _models.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetAllByLevel(int level)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(t=>t.Level == level)
                            .OrderByDescending(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }
       
        public IEnumerable<ArticleCategory> GetAllCreateBy(string userName)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Query()
                            .Select()
                            .Where(t=>t.CreatedBy == userName)
                            .OrderByDescending(t => t.CreatedDate); 

            return _models.AsEnumerable();
        }

        public IEnumerable<ArticleModel> GetArticles(Guid categoryId)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ArticleModel>().Query().Select()
                .Where(t=>t.CategoryId != null && t.CategoryId.HasValue && t.CategoryId.Value == categoryId)
                .OrderByDescending(t => t.CreatedDate);

            return _models.AsEnumerable();
        }
    }
}