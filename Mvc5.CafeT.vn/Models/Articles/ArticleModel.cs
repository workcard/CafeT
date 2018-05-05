
using CafeT.SmartObjects;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Web.Models;

namespace Mvc5.CafeT.vn.Models
{
    public enum PublishStatus
    {
        IsPublished,
        IsDrafted
    }
    public enum ScopeStatus
    {
        IsPublic,
        IsProtect,
        IsPrivate
    }
    public class ArticleModel:BaseObject
    {
        [Required]
        [Display(Name = "Tiêu đề")]
        public string Title { set; get; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Tóm tắt")]
        public string Summary { set; get; }

        [Required]
        [Display(Name = "Nội dung")]
        public string Content { set; get; }
        [Display(Name = "Tags")]
        public string Tags { set; get; }
        public List<string> Authors { set; get; }

        public PublishStatus Status { set; get; } = PublishStatus.IsDrafted;
        public ScopeStatus Scope { set; get; } = ScopeStatus.IsPublic;

        [Display(Name = "Hình đại diện")]
        public string AvatarPath { set; get; }

        public virtual string[] GetKeywords()
        {
            TextProcessor processor = new TextProcessor(Content);
            var _keywords = processor.GetVnKeywords();
            if (!this.Tags.IsNullOrEmptyOrWhiteSpace() && this.Tags.Length > 0)
            {
                _keywords.ToList().AddRange(this.Tags.ToWords());
            }
            return _keywords.Distinct().ToArray();
        }

        public bool IsMatch(string keyword)
        {
            keyword = keyword.ToLower();
            if ((!this.Title.IsNullOrEmptyOrWhiteSpace()
                && this.Title.ToLower().Contains(keyword))
                || ((!this.Summary.IsNullOrEmptyOrWhiteSpace()
                && this.Summary.ToLower().Contains(keyword))
                || ((!this.Content.IsNullOrEmptyOrWhiteSpace()
                && this.Content.ToLower().Contains(keyword)))
                || ((!this.Tags.IsNullOrEmptyOrWhiteSpace()
                && this.Tags.ToLower().Contains(keyword)))
                ))
            {
                return true;
            }
            return false;
        }

        public bool HasYouTubeLink()
        {
            var _youtubeUrls = this.Content.GetYouTubeUrls();
            if (_youtubeUrls != null && _youtubeUrls.Length > 0) return true;
            return false;
        }

        public string[] GetYouTubeUrls()
        {
            var _youtubeUrls = this.Content.GetYouTubeUrls();
            return _youtubeUrls;
        }

        public string EnglishContent { set; get; }
        public string VietnameseContent { set; get; }
        public int? Points { set; get; }

        public Guid? CourseId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? CompanyId { set; get; }
        public Guid? JobId { set; get; }
        [Display(Name = "Phân loại")]
        public Guid? CategoryId { set; get; }

        public string Followers { set; get; }
        public string FromUrl { set; get; }

        //public virtual ImageModel Avatar { set; get; }
        public virtual ArticleCategory Category { set; get; }
        public virtual IEnumerable<QuestionModel> Questions { set; get; }
        public virtual IEnumerable<CommentModel> Comments { set; get; }
        public virtual IEnumerable<FileModel> Files { set; get; }
        public int CountViews { get; set; }

        public ArticleModel() : base()
        {
            Points = 0;
        }

        public ArticleModel ToStandard()
        {
            Title = Title.ToStandard();
            Summary = Summary.ToStandard();
            Content = Content.ToStandard();
            return this;
        }

        public bool IsPublished()
        {
            if (Status == PublishStatus.IsPublished)
                return true;
            else
                return false;
        }

        public void ToView(string userName)
        {
            CountViews = CountViews + 1;
        }
    }
}