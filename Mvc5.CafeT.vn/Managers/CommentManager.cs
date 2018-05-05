using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Managers
{
    public class CommentManager:ObjectManager
    {
        private readonly CommentService _commentService;

        public CommentManager(CommentService service, IUnitOfWorkAsync unitOfWorkAsync):base(unitOfWorkAsync)
        {
            _commentService = service;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public IEnumerable<CommentModel> GetAll()
        {
            return _commentService.GetAll();
        }

        public IEnumerable<CommentModel> GetAllByArticle(Guid id)
        {
            var _comments = _commentService.GetAll()
                .Where(t => t.IsOfArticle(id))
                .OrderByDescending(m => m.CreatedDate);
            return _comments;
        }

        public IEnumerable<CommentModel> GetAllByUser(string userName)
        {
            var _comments = _unitOfWorkAsync.RepositoryAsync<CommentModel>().Query().Select()
                .Where(t => t.CreatedBy == userName);
            return _comments;
        }

        public CommentModel GetById(Guid id)
        {
            return _commentService.GetById(id);
        }

        public bool Update(CommentModel model)
        {
            try
            {
                _commentService.Update(model);
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Insert(CommentModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<CommentModel>().Insert(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool Delete(CommentModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<CommentModel>().Delete(model);
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
        
        //public void Notify(string[] users)
        //{
        //    foreach(string user in users)
        //    {
        //        Notify(user);   
        //    }
        //}

        //public void Notify(string user)
        //{

        //}
    }
}