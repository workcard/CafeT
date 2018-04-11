using CafeT.Html;
using CafeT.Objects;
using CafeT.Text;
using System;


namespace Web.Models
{
    public class Url : BaseObject
    {
        public string Domain { set; get; }
        public string Address { set; get; }
        public string Title { set; get; }
        public string HtmlContent { set; get; }

        public Guid? IssueId { set; get; }
        public Guid? QuestionId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? CommentId { set; get; }
        public Guid? StoryId { set; get; }
        public Guid? AnswerId { set; get; }

        public DateTime? LastestCheck { set; get; }

        //public bool IsLive { set; get; }
        public Url() : base() { }

        public Url(string url):base()
        {
            if (url.IsUrl())
            {
                Address = url;
                Uri myUri = new Uri(url);
                string host = myUri.Host;
                Domain = host;
            }
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