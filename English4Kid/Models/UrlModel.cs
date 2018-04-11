using CafeT.BusinessObjects;
using CafeT.Html;
using CafeT.Text;
using System;

namespace MathBot.Models
{
    public enum UrlType
    {
        Undefinition,
        ImageLink,
        HtmlLink,
        DownloadLink,
        YouTubeLink,
        DropBoxLink,
        MicrosoftDriveLink,
        GooogleDriveLink
    }

    public class UrlModel : BaseObject
    {
        public string Url { set; get; }
        public string Host { set; get; }
        public string Domain { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string HtmlContent { set; get; } = string.Empty;
        public UrlType Type { set; get; }

        public DateTime? LastRead { set; get; }

        public Guid? ArticleId { set; get; }
        public Guid? QuestionId { set; get; }
        public Guid? InterviewId { set; get; }
        public Guid? AnswerId { set; get; }
        public Guid? CourseId { set; get; }


        public UrlModel() : base()
        {
            Url = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
        }

        public UrlModel(string url):base()
        {
            if(url.IsUrl())
            {
                Url = url;
                Host = url.GetHost();
                Domain = url.GetDomain();
            }
        }

        public bool IsLoaded()
        {
            if (HtmlContent.IsNullOrEmptyOrWhiteSpace()) return false;
            return true;
        }

        public void LoadFull()
        {
            if(!IsLoaded())
            {
                LoadHtmlContent();
            }
        }

        protected string LoadHtmlContent()
        {
            if (HtmlContent.IsNullOrEmptyOrWhiteSpace())
            {
                HtmlContent = Url.LoadHtmlAsync().Result;
                LastRead = DateTime.Now;
            }
            return HtmlContent;
        }

        public bool IsYouTubeLink()
        {
            if (Url.IsYouTubeUrl()) return true;
            return false;
        }

        public bool IsYouTubeWatchLink()
        {
            if (Url.IsYouTubeWatchUrl()) return true;
            return false;
        }

        public bool IsHtmlLink()
        {
            if (Url.IsHtmlLink()) return true;
            return false;
        }
        public UrlType GetUrlType()
        {
            if (Url.IsImageUrl()) return UrlType.ImageLink;
            if (Url.IsHtmlLink()) return UrlType.HtmlLink;
            if (Url.IsYouTubeUrl()) return UrlType.YouTubeLink;
            return UrlType.Undefinition;
        }
        public bool IsImageLink()
        {
            if (Url.IsImageUrl()) return true;
            return false;
        }

        public bool IsOfArticle(Guid id)
        {
            if (ArticleId.HasValue && ArticleId.Value == id) return true;
            return false;
        }
        public bool IsOfQuestion(Guid id)
        {
            if (QuestionId.HasValue && QuestionId.Value == id) return true;
            return false;
        }
        public bool IsOfInterview(Guid id)
        {
            if (InterviewId.HasValue && InterviewId.Value == id) return true;
            return false;
        }
        public bool IsOfAnswer(Guid id)
        {
            if (AnswerId.HasValue && AnswerId.Value == id) return true;
            return false;
        }
        public bool IsOfCourse(Guid id)
        {
            if (CourseId.HasValue && CourseId.Value == id) return true;
            return false;
        }
    }
}