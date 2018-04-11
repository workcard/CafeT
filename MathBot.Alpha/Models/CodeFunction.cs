
using CafeT.BusinessObjects;
using CafeT.Html;
using CafeT.Objects;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MathBot.Models
{
    public class CodeFunction : BaseObject
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public string[] Inputs { set; get; }
        public string FullBody { set; get; }
        public string[] Outputs { set; get; }

        //public bool IsPassed { set; get; } = false;
        //public DateTime? LastestCheck { set; get; }

        public CodeFunction() : base() { }

        //public Url(string url):base()
        //{
        //    if (url.IsUrl())
        //    {
        //        Address = url;
        //        Uri myUri = new Uri(url);
        //        string host = myUri.Host;
        //        Domain = host;
        //    }
        //}

        //public bool IsLive()
        //{
        //    GetDomain();
        //    if(!Domain.IsNullOrEmptyOrWhiteSpace())
        //    {
        //        ComputerObject _computer = new ComputerObject();
        //        return _computer.IsConnectedToUrl(Domain);
        //    }
        //    return false;
        //}

        //public void GetDomain()
        //{
        //    if (Address.IsUrl())
        //    {
        //        Address = Address;
        //        Uri myUri = new Uri(Address);
        //        string host = myUri.Host;
        //        Domain = host;
        //    }
        //}
        //public void Fetch()
        //{
        //    HtmlContent = this.Address.LoadHtml().Result;
        //}
    }
}