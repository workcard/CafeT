using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Translators
{
    public class GoogleServices
    {
        public GoogleServices()
        {
        }

        public string GetGoogleApiKey()
        {
            return "AIzaSyDz0FsFxxf7xsskqxlhaNWMvxM05b4HQBc";
        }
        public string GetGoogleSearchEngine()
        {
            return "004317969426278842680:_f5wbsgg7xc";
        }

        public Google.Apis.Customsearch.v1.Data.Search Search(string keywords)
        {
            List<string> _results = new List<string>();

            var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer
            {
                ApiKey = GetGoogleApiKey()
            });

            var listRequest = svc.Cse.List(keywords);

            listRequest.Cx = GetGoogleSearchEngine();
            //listRequest.Gl = "vn"; //Default search by Vietnamese
            //listRequest.Hl = "vi";

            //string startDate = DateTime.UtcNow.AddDays(-3).ToString("yyyyMMdd");
            ////string startDate = (DateTime.UtcNow.Year.ToString() + sMonth + sday);
            //string endDate = DateTime.UtcNow.ToString("yyyyMMdd");
            //listRequest.DateRestrict = startDate + ":" + endDate;
            //listRequest.Start = 1;
            var search = listRequest.Execute();//.Fetch();
            return search;
        }

        public Google.Apis.Customsearch.v1.Data.Search SearchImage(string keywords)
        {
            List<string> _results = new List<string>();
            var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer
            {
                ApiKey = GetGoogleApiKey()
            });
            var listRequest = svc.Cse.List(keywords);

            listRequest.Cx = GetGoogleSearchEngine();

            string startDate = DateTime.UtcNow.AddDays(-3).ToString("yyyyMMdd");
            //string startDate = (DateTime.UtcNow.Year.ToString() + sMonth + sday);
            string endDate = DateTime.UtcNow.ToString("yyyyMMdd");
            listRequest.DateRestrict = startDate + ":" + endDate;
            listRequest.Start = 1;
            var search = listRequest.Execute();//.Fetch();
            return search;
        }
    }
}
