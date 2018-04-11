using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CafeT.GoogleServices
{
    public class MicrosoftService
    {
        public async Task<string> Search(string keywords)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "f290b58e86bb4bb88806e5a93cd2cbf6");

            // Request parameters
            queryString["q"] = keywords;
            queryString["count"] = "10";
            queryString["offset"] = "0";
            //queryString["mkt"] = "en-us";
            //queryString["safesearch"] = "Moderate";
            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/search?" + queryString;

            using (var r = client.GetAsync(new Uri(uri)).Result)
            {
                string result = await r.Content.ReadAsStringAsync();
                Rootobject LegList = JsonConvert.DeserializeObject<Rootobject>(result);

                StringBuilder sb = new StringBuilder();

                foreach (var item in LegList.WebPages.Value)
                {
                    //sb.Append("<li><a href=" + item.DisplayUrl + ">" + item.Name + "</a><br/><span>" + item.snippet + " </span></li>");
                }
                return sb.ToString();
            }
        }
    }

    internal class Rootobject
    {
        public WebPage WebPages { get; internal set; }

        public class WebPage
        {
            public IEnumerable<object> Value { get; internal set; }
        }
    }
}
