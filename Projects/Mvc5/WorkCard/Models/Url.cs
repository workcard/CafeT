using CafeT.Html;
using CafeT.Objects;
using CafeT.Text;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Web.Models
{
    public class Url : BaseObject
    {
        public string Domain { set; get; }
        public string Address { set; get; }
        public string Title { set; get; }
        public string AvatarUrl { set; get; }
        public string HtmlContent { set; get; }
        public string CssTitle { set; get; }
        public string CssContent { set; get; }
        public string CssInfo { set; get; }
        public string CssDescription { set; get; }
        public string CssAvatar { set; get; }
        public Guid? IssueId { set; get; }
        public Guid? QuestionId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? CommentId { set; get; }
        public Guid? StoryId { set; get; }
        public Guid? AnswerId { set; get; }

        public DateTime? LastestCheck { set; get; }

        [NotMapped]
        private WebPage page;
        public WebPage Page { get => page; set => page = value; }

        public Url() : base() { }

        public Url(string url):base()
        {
            if (url.IsUrl())
            {
                Address = url;
                Uri myUri = new Uri(url);
                string host = myUri.Host;
                Domain = host;
                Page = new WebPage(url);
            }
        }
        [NotMapped]
        public bool IsLoaded { set; get; } = false;
        

        public void Load()
        {
            if(Address.IsUrl())
            {
                Uri myUri = new Uri(Address);
                string host = myUri.Host;
                Domain = host;
                Page = new WebPage(Address);
                LoadTitle();
                IsLoaded = true;
            }
        }
        public void LoadTitle()
        {
            if(!page.IsLoaded)
            Page.Load();
            if(!CssTitle.IsNullOrEmptyOrWhiteSpace())
            Title = Page.GetNodesByClass(CssTitle)
                    .FirstOrDefault()
                    .InnerText.ToStandard();
            AvatarUrl = Page.GetNodesByClass(CssAvatar)
                    .FirstOrDefault()
                    .InnerHtml.GetImages().FirstOrDefault();

        }

        public bool IsLive()
        {
            GetDomain();
            if(!Domain.IsNullOrEmptyOrWhiteSpace())
            {
                ComputerObject _computer = new ComputerObject();
                return _computer.IsConnectedToUrl(Domain);
            }
            return false;
        }

        public void GetDomain()
        {
            if (Address.IsUrl())
            {
                Address = Address;
                Uri myUri = new Uri(Address);
                string host = myUri.Host;
                Domain = host;
            }
        }
        public void Fetch()
        {
            HtmlContent = this.Address.LoadHtmlAsync().Result;
        }
    }
}