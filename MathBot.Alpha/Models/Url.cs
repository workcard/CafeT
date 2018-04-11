
using CafeT.BusinessObjects;
using CafeT.Html;
using CafeT.Objects;
using CafeT.Text;
using System;


namespace MathBot.Models
{
    //public class UrlModel : BaseObject
    //{
    //    public string Domain { set; get; }
    //    public string Address { set; get; }
    //    public string Title { set; get; }
    //    public string HtmlContent { set; get; }

    //    public DateTime? LastestCheck { set; get; }

    //    public UrlModel() : base() { }

    //    public UrlModel(string url):base()
    //    {
    //        if (url.IsUrl())
    //        {
    //            Address = url;
    //        }
    //    }

    //    public bool IsLive()
    //    {
    //        GetDomain();
    //        if(!Domain.IsNullOrEmptyOrWhiteSpace())
    //        {
    //            ComputerObject _computer = new ComputerObject();
    //            return _computer.IsConnectedToUrl(Domain);
    //        }
    //        return false;
    //    }

    //    public void GetDomain()
    //    {
    //        if (Address.IsUrl())
    //        {
    //            Address = Address;
    //            Uri myUri = new Uri(Address);
    //            string host = myUri.Host;
    //            Domain = host;
    //        }
    //    }
    //    public void Fetch()
    //    {
    //        HtmlContent = this.Address.LoadHtmlAsync().Result;
    //    }
    //}
}