using CafeT.SmartObjects;
using CafeT.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CafeT.BusinessObjects
{
    public class Article:BaseObject
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

        public Article():base()
        {
            Authors = new List<string>();
            Authors.Add(CreatedBy);
        }

        public virtual string[] GetKeywords()
        {
            TextProcessor processor = new TextProcessor(Content);
            var _keywords = processor.GetVnKeywords();
            if(!this.Tags.IsNullOrEmptyOrWhiteSpace() && this.Tags.Length > 0)
            {
                _keywords.ToList().AddRange(this.Tags.ToWords());
            }
            return _keywords.Distinct().ToArray();
        }

        public bool IsMatch(string keyword)
        {
            keyword = keyword.ToLower();
            if((!this.Title.IsNullOrEmptyOrWhiteSpace()
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
    }
}
