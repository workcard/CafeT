using CafeT.BusinessObjects;
using CafeT.Html;
using CafeT.SmartObjects;
using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class ArticleInfo
    {
        public int CountOfWords { set; get; } = 0;
        public int TimeToRead { set; get; } = 0; //Minutes
        public string[] AutoKeywords { set; get; }

        public string[] Urls { set; get; }
        public string[] Images { set; get; }
        public string[] YouTubeLinks { set; get; }
        public string[] EnglishWords { set; get; }
        public string[] Commands { set; get; }
    }

    public class ArticleView : BaseView
    {
        [Display(Name = "Tiêu đề")]
        public string Title { set; get; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mô tả")]
        public string Summary { set; get; }
        [Display(Name = "Nội dung")]
        public string Content { set; get; }
        public int? Points { set; get; }
        public PublishStatus Status { set; get; } = PublishStatus.IsDrafted;
        public ScopeStatus Scope { set; get; } = ScopeStatus.IsPublic;

        public string Followers { set; get; }

        public int CountViews { set; get; }
        [Display(Name = "Nội dung Tiếng Anh")]
        public string EnglishContent { set; get; }
        [Display(Name = "Nội dung")]
        public string VietnameseContent { set; get; }
        public CommentModel Comment { set; get; }
        public QuestionModel QuestionCreate { set; get; }
        public List<CommentModel> Comments { set; get; }
        public List<QuestionModel> Questions { set; get; }
        public List<FileModel> Files { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? ProjectId { set; get; }
        [Display(Name = "Phân loại")]
        public Guid? CategoryId { set; get; }
        public virtual ArticleCategory Category { set; get; }
        public string Tags { set; get; }
        public ApplicationUser Author { set; get; }
        public string ImageAuthor { set; get; }
        public List<ImageModel> ImageModels { get; set; }
        public List<string> Images { set; get; } = new List<string>();
        public List<string> OldImages { set; get; } = new List<string>();
        [Display(Name = "Hình đại diện")]
        public string AvatarPath { set; get; }

        public TextProcessor Processor;
        public ArticleView()
        {
            Files = new List<FileModel>();
            Author = new ApplicationUser();
            ImageModels = new List<ImageModel>();
        }

        public ArticleView(ArticleModel model)
        {
            Files = new List<FileModel>();
            Author = new ApplicationUser();
            ImageModels = new List<ImageModel>();
            Id = model.Id;
            ProjectId = model.ProjectId;
            CourseId = model.CourseId;
            CreatedDate = model.CreatedDate;
            CreatedBy = model.CreatedBy;
            LastUpdatedBy = model.LastUpdatedBy;
            LastUpdatedDate = model.LastUpdatedDate;
            Title = model.Title;
            Scope = model.Scope;
            Status = model.Status;
            Followers = model.Followers;
            CategoryId = model.CategoryId;
            Summary = model.Summary;
            Content = model.Content;
            CountViews = model.CountViews;
            Points = model.Points;
            
            AvatarPath = model.AvatarPath;
            if (model.Tags != null)
            {
                Tags = model.Tags;
            }
            else
            {
                Tags = model.Title.ToWords().First();
            }
            Processor = new TextProcessor(Content);
            VietnameseContent = Processor.Output;
            EnglishContent = model.EnglishContent;
        }

        public string GetDescription(int n)
        {
            if (this.Summary.IsNullOrEmptyOrWhiteSpace()) return this.Summary.GetNWords(n);
            return this.Content.GetNWords(n);
        }
        public string[] GetKeyWords()
        {
            TextProcessor processor = new TextProcessor(Title);
            string[] _keywords = processor.GetVnKeywords();
            string[] _tags = Tags.Split(new string[] { ";", "," }, StringSplitOptions.None)
                .Where(t=> !t.IsNullOrEmptyOrWhiteSpace())
                .ToArray();
            return _keywords.AsEnumerable().Union(_tags.AsEnumerable())
                .Distinct()
                .ToArray();
        }

        public void ResizeImages()
        {
            OldImages = this.Content.GetImages().ToList();
            foreach(string img in OldImages)
            {
                string newImg = img.Replace(">","");
                string _width = @"width=" + "100%";
                string _heigh = @"height=" + "auto";
                if (!img.Contains("width"))
                {
                    newImg = newImg + _width;
                }
                if(!img.Contains("height"))
                {
                    newImg = newImg + _heigh;
                }
                newImg = newImg + ">";
                Images.Add(newImg);
                Content = Content.Replace(img, newImg);
            }
        }
        public string Avatar { set; get; }
        public void SetAvatar()
        {
            ResizeImages("180", "800");
            Avatar = Images.FirstOrDefault();
        }
        public void ResizeImages(string height, string width)
        {
            OldImages = Content.GetImages().ToList();
            foreach (string img in OldImages)
            {
                string newImg = img.Replace(">", "");
                string _width = @"width=" + width + "\"";
                string _heigh = @" height=" + height;
                if (!img.Contains("width"))
                {
                    newImg = newImg + _width;
                }
                if (!img.Contains("height"))
                {
                    newImg = newImg + _heigh;
                }
                newImg = newImg + " class=\"img-responsive\" " +  ">";
                Images.Add(newImg);
                Content = Content.Replace(img, newImg);
            }
        }

        public bool CanView()
        {
            if (Title.ToWords().Length <= 2)
            {
                return false;
            }
            if (Content.ToWords().Length <= 2) return false;
            return true;
        }

        public bool IsNews()
        {
            if(this.CreatedDate.AddDays(3) >= DateTime.Now)
            {
                return true;
            }
            return false;
        }

        public bool HasFiles()
        {
            if (Files != null && Files.Count > 0) return true;
            return false;
        }

        public void ToView()
        {
            var _youTubeUrls = Content.GetYouTubeUrls();
            foreach (string _link in _youTubeUrls)
            {
                YouTubeView _youTube = new YouTubeView(_link);
                Content = Content.Replace(_link, _youTube.EmbedUrl);
            }
        }
    }
}