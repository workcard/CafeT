using CafeT.Enumerable;
using CafeT.Html;
using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc5.CafeT.vn.Managers
{
    public class ArticleManager:ObjectManager
    {
        public ArticleManager(IUnitOfWorkAsync unitOfWorkAsync):base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }
        public ArticleCategory GetCategory(Guid id)
        {
            return _unitOfWorkAsync.RepositoryAsync<ArticleCategory>()
                .Query().Select().Where(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<ArticleModel> GetAll()
        {
            var models = _unitOfWorkAsync.RepositoryAsync<ArticleModel>()
                .Query().Select()
                .OrderByDescending(t=>t.CreatedDate);
            return models;
        }

        public IEnumerable<string> GetAllYouTubes()
        {
            List<string> YouTubeUrls = new List<string>();

            var _articles = _unitOfWorkAsync.RepositoryAsync<ArticleModel>()
                .Query().Select();

            int _count = _articles.Count();

            if (_articles != null && _count > 0)
            {
                foreach (var item in _articles)
                {
                    var _youTubes = item.Content.GetYouTubeWatchUrls().ToList();
                    if (_youTubes != null && _youTubes.Count > 0)
                    {
                        YouTubeUrls.AddRange(_youTubes);
                    }
                }
            }
            return YouTubeUrls.Distinct().AsEnumerable();
        }

        //public IEnumerable<ArticleModel> SearchBy(string searchString)
        //{
        //    var _objects = _articleService.GetAll();
        //    _objects = _objects.Where(s => s.IsMatch(searchString))
        //                    .OrderByDescending(t => t.CreatedDate)
        //                    .ToList();
        //    return _objects;
        //}

        public IEnumerable<ArticleModel> GetAllPublished()
        {
            var _articles = GetAll()
                .Where(t => t.Status == PublishStatus.IsPublished)
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }

        public IEnumerable<ArticleModel> GetAllUnPublished()
        {
            var _articles = GetAll()
                .Where(t => t.Status != PublishStatus.IsPublished)
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }

        public IEnumerable<ArticleModel> GetAllPublished(string createBy)
        {
            var _articles = GetAll()
                .Where(t => t.Status == PublishStatus.IsPublished && t.CreatedBy == createBy)
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }

        public IEnumerable<ArticleModel> GetTopViews(int? n)
        {
            var _articles = GetAllPublished().TakeMax(n);
            return _articles;
        }

        public async Task<IEnumerable<ArticleModel>> RelatedAsync(Guid id)
        {
            var _article = await GetByIdAsync(id);
            var _keywords = _article.GetKeywords();
            var _articles = GetAllPublished().Where(t=>t.Id != id);
            var _related = _articles.Where(t => !t.Tags.IsNullOrEmptyOrWhiteSpace() 
                                            && !_article.Tags.IsNullOrEmptyOrWhiteSpace()
                                            && t.Tags.IntersectWords(_article.Tags).Count() > 1);
            return _related;
        }

        
        public IEnumerable<ArticleModel> GetAllDrafted()
        {
            var _articles = GetAll()
                .Where(t => !t.IsPublished())
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }

        public IEnumerable<ArticleModel> GetAllDrafted(string createBy)
        {
            var _articles = GetAll()
                .Where(t => t.IsOf(createBy) && !t.IsPublished())
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }

        public IEnumerable<string> GetAllTags()
        {
            List<string> _strs = new List<string>();
            var _tags = GetAllTags();
            return _strs.Distinct();
        }
        
        public async Task<ArticleModel> GetByIdAsync(Guid id)
        {
            return await _unitOfWorkAsync.RepositoryAsync<ArticleModel>().FindAsync(id);
        }
        
        //public List<string> Process(Guid id)
        //{
        //    List<string> _objects = new List<string>();
        //    var _article = _articleService.GetById(id);
        //    if(!_article.Content.IsNullOrEmptyOrWhiteSpace())
        //    {
        //        string[] _commands = _article.Content.ExtractAllBetween("@{", "}@");
        //        if (_commands != null && _commands.Count() > 0)
        //        {
        //            foreach (string _command in _commands)
        //            {
        //                _objects = this.GetAll().Select(t => t.Title).ToList();
        //            }
        //        }
        //    }
            
        //    return _objects;
        //}

        public System.Drawing.Image Base64ToImage(string base64String)
        {

            string _data = base64String.DeleteBeginTo("base64,").Replace("base64,", string.Empty).DeleteEndTo("\" />");
            var binData = Convert.FromBase64String(_data);
            Bitmap _image;
            using (var stream = new MemoryStream(binData))
            {
                _image = new Bitmap(stream);
            }
            return _image;
        }

        public string[] GetInnerImages(Guid id, string imgSourcePath)
        {
            List<string> images = new List<string>();
            var _article = GetByIdAsync(id).Result;
            if(_article != null)
            {
                List<string> _images = new List<string>();
                _images = _article.Content.GetImages().ToList();
                if (_images != null && _images.Count() > 0)
                {
                    int i = 0;
                    foreach (var _img in _images)
                    {
                        if (_img.Contains("data:image"))
                        {
                            string _imagePath = imgSourcePath + id.ToString() + i.ToString();

                            Image _image = Base64ToImage(_img);
                            _image = ResizeImage(_image, new Size() { Width = 100, Height = 150 });
                            _image.Save(_imagePath + ".png", ImageFormat.Png);
                            images.Add(_imagePath + ".png");
                        }
                        i = i + 1;
                    }
                }
                return images.ToArray();
            }
            return null;
        }

        public Image ResizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        //public void ProcessUrls(Guid id)
        //{
        //    var _article = _articleService.GetById(id);
        //    var _urls = _article.Content.GetUrlsWithoutHref();
        //    if(_urls != null && _urls.Count() >0)
        //    {
        //        foreach(string _url in _urls)
        //        {
        //            if(_url.IsUrl())
        //            {
        //                UrlModel _object = new UrlModel(_url);
        //                new UrlManager(_unitOfWorkAsync).Insert(_object);
        //            }
        //        }
        //    }
        //}

        public ArticleModel GetToView(Guid id, string userView)
        {
            var _article = _unitOfWorkAsync.RepositoryAsync<ArticleModel>().
                Find(id);
            _article.ToView(userView);
            Update(_article);
            return _article;
        }

        public bool Update(ArticleModel article)
        {
            _unitOfWorkAsync.RepositoryAsync<ArticleModel>().
                Update(article);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
            
        }
        public async Task<bool> DeleteAsync(ArticleModel article)
        {
            try
            {
                await _unitOfWorkAsync.RepositoryAsync<ArticleModel>().DeleteAsync(article);
                await _unitOfWorkAsync.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool Insert(ArticleModel article)
        {
            _unitOfWorkAsync.RepositoryAsync<ArticleModel>().Insert(article);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool AddQuestion(QuestionModel question)
        {
            _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Insert(question);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
        }

        public IEnumerable<QuestionModel> GetQuestions(Guid id)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(c => c.ArticleId == id);
            return _models.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetVerifiedQuestions(Guid articleId)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(c => c.IsOfArticle(articleId) && c.IsVerified);

            return _models.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetUnVerifyQuestions(Guid id)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(c => c.IsOfArticle(id) && !c.IsVerified);

            return _models.AsEnumerable();
        }

        public IEnumerable<FileModel> GetFiles(Guid id)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<FileModel>().Query()
                            .Select()
                            .Where(c => c.ArticleId != null && c.ArticleId.HasValue && c.ArticleId.Value == id);

            return _models.AsEnumerable();
        }

        public bool AddComment(Guid id, CommentModel comment)
        {
            if(comment.ArticleId != null && comment.ArticleId.HasValue && comment.ArticleId.Value == id)
            {
                _unitOfWorkAsync.RepositoryAsync<CommentModel>().Insert(comment);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    this.ErrorMessage = ex.Message;
                    return false;
                }
            }
            return false;
        }

        public bool AddFile(Guid id, FileModel model)
        {
            if (model.ArticleId != null && model.ArticleId.HasValue && model.ArticleId.Value == id)
            {
                var _exitsFiles = GetFiles(id).Select(t=>t.FileName);
                if (_exitsFiles.Contains(model.FileName)) return false;
                _unitOfWorkAsync.RepositoryAsync<FileModel>().Insert(model);
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
            return false;
        }
    }
}