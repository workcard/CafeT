using CafeT.Text;
using Mvc5.CafeT.vn.Managers;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.ModelViews;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Mappers
{
    public class Mappers
    {
        protected readonly ArticleManager _manager;

        public Mappers(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _manager = new ArticleManager(unitOfWorkAsync);
        }

        #region Articles
        public ArticleModel ToModel(ArticleView view)
        {
            ArticleModel _model = new ArticleModel();
            _model.Id = view.Id;
            _model.ProjectId = view.ProjectId;
            _model.CourseId = view.CourseId;
            _model.Tags = view.Tags;
            _model.CreatedBy = view.CreatedBy;
            _model.CreatedDate = view.CreatedDate;
            _model.UpdatedDate = view.UpdatedDate;
            _model.UpdatedBy = view.UpdatedBy;
            _model.Title = view.Title;
            _model.Summary = view.Summary;
            _model.Content = view.Content;
            _model.EnglishContent = view.EnglishContent;
            _model.VietnameseContent = view.VietnameseContent;
            _model.Followers = view.Followers;
            _model.CategoryId = view.CategoryId;
            _model.Status = view.Status;
            _model.Scope = view.Scope;
            _model.AvatarPath = view.AvatarPath;
            _model.CountViews = view.CountViews;

            return _model;
        }
        #endregion

        //public static FileModel GoogleFileToModel(Google.Apis.Drive.v3.Data.File file)
        //{
        //    FileModel fileModel = new FileModel();
        //    fileModel.Id = Guid.NewGuid();
        //    fileModel.FileName = file.Name;
        //    if (file.HasThumbnail.HasValue && file.HasThumbnail.Value)
        //        fileModel.AvatarPath = file.ThumbnailLink;
        //    fileModel.FullPath = file.Id;
        //    fileModel.SizeInB = file.Size.Value;
        //    fileModel.Description = file.Description;
        //    fileModel.CreatedBy = file.Owners[0].DisplayName;
        //    return fileModel;
        //}

        public ArticleModel ToArticle(WebPageModel model)
        {
            ArticleModel _view = new ArticleModel();
            _view.Id = model.Id;
            _view.CreatedDate = model.CreatedDate;
            _view.CreatedBy = model.CreatedBy;
            _view.Title = model.Title;
            _view.Content = model.Page.HtmlContent;

            if (model.Title != null)
            {
                _view.Tags = model.Title.ToWords().First();
            }
            else
            {
                _view.Tags = string.Empty;
            }
            return _view;
        }

        public  ArticleView ToView(ArticleModel model)
        {
            ArticleView _view = new ArticleView(model);
            if (model.CategoryId.HasValue)
            {
                _view.Category = _manager.GetCategory(model.CategoryId.Value);
            }
            _view.SetAvatar();
            _view.ResizeImages();
            return _view;
        }

        public List<ArticleView> ToViews(List<ArticleModel> models)
        {
            List<ArticleView> _views = new List<ArticleView>();
            foreach (var model in models)
            {
                var _view = ToView(model);
                if(_view != null)
                {
                    _views.Add(_view);
                }
                else
                {
                    System.Console.WriteLine("Can not convert to view of " + model.Title);
                }
            }
            return _views.ToList();
        }

        //public ExamView ToView(ExamModel model)
        //{
        //    ExamView _view = new ExamView();
        //    _view.Id = model.Id;
        //    _view.CreatedDate = model.CreatedDate;
        //    _view.CreatedBy = model.CreatedBy;
        //    _view.Name = model.Name;
        //    _view.Description = model.Description;
        //    _view.QuestionCreate = new QuestionModel();
        //    _view.QuestionCreate.ExamId = model.Id;
        //    _view.CountViews = model.CountViews;
        //    return _view;
        //}

       

        
        public CommentModel ToModel(CommentView view)
        {
            CommentModel _model = new CommentModel();
            _model.Id = view.Id;
            _model.ArticleId = view.ArticleId;
            _model.ProjectId = view.ProjectId;
            _model.CourseId = view.CourseId;            
            _model.CreatedBy = view.CreatedBy;
            _model.CreatedDate = view.CreatedDate;
            _model.UpdatedDate = view.UpdatedDate;
            _model.UpdatedBy = view.UpdatedBy;
            _model.Title = view.Title;            
            _model.Content = view.Content;
            _model.CountViews = view.CountViews;

            return _model;
        }
        public CommentView ToView(CommentModel model)
        {
            if (model is null) return null;
            CommentView _view = new CommentView();
            _view.Id = model.Id;
            _view.ProjectId = model.ProjectId;
            _view.CourseId = model.CourseId;
            _view.CreatedDate = model.CreatedDate.Value;
            _view.CreatedBy = model.CreatedBy;
            _view.UpdatedBy = model.UpdatedBy;
            _view.UpdatedDate = model.UpdatedDate;
            _view.Title = model.Title;
            _view.Content = model.Content;
            _view.CountViews = model.CountViews;
            _view.Files = _manager.GetFiles(model.Id).ToList();
            _view.ArticleId = model.ArticleId;
            if(model.ArticleId.HasValue)
            {
                _view.Article = _manager.GetByIdAsync(model.ArticleId.Value).Result;
            }
            
            return _view;
        }
        public List<CommentView> ToViews(List<CommentModel> models)
        {
            List<CommentView> _views = new List<CommentView>();
            foreach (var model in models)
            {
                _views.Add(ToView(model));
            }
            return _views.ToList();
        }
    }
}