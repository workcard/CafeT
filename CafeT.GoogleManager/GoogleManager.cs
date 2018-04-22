using CafeT.Objects;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using Google.Apis.Translate.v2;
using Google.Apis.Translate.v2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CafeT.GoogleManager
{
    

    public class GoogleManager
    {
        public string Key { set; get; }
        public string ApplicationName { set; get; }
        public string EngineKey { set; get; }
        public string TargetLangCode = "vi";
        public string SourceLangCode = "en";

        public GoogleManager()
        {
        }

        //public GoogleManager(string apiKey, String? searchEngineKey, string applicationName)
        //{
        //    Key = apiKey;
        //    EngineKey = searchEngineKey;
        //}

        private CseResource.ListRequest MakeSearchRequest(string keywords)
        {
            var svc = new CustomsearchService(new BaseClientService.Initializer
            {
                ApiKey = Key
            });

            var listRequest = svc.Cse.List(keywords);
            listRequest.Cx = EngineKey;
            return listRequest;
        }

        //private Google.Apis.Translate.v2.TranslationsResource.ListRequest MakeTranlationsRequest(string keywords)
        //{
        //    var svc = new TranslateService(new BaseClientService.Initializer
        //    {
        //        ApiKey = Key
        //    });

        //    var listRequest = svc.Translations.List(keywords,TargetLangCode);
        //    listRequest.Key = EngineKey;
        //    return listRequest;
        //}

        //public string Translate(string text, string targetLang)
        //{
        //    var request = MakeTranlationsRequest(text);
        //    TranslationsListResponse response = request.Execute();
        //    return response.Translations.Select(t => t.TranslatedText).ToArray()[0];
        //}

        public Search Search(string keywords)
        {
            List<string> _results = new List<string>();
            CseResource.ListRequest listRequest = MakeSearchRequest(keywords);
            listRequest.Gl = "en";
            listRequest.Hl = "vi";

            try
            {
                var search = listRequest.Execute();
                return search;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Search SearchImage(string keywords)
        {
            List<string> _results = new List<string>();
            CseResource.ListRequest listRequest = MakeSearchRequest(keywords);
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.ImgType = CseResource.ListRequest.ImgTypeEnum.Photo;
            listRequest.ImgSize = CseResource.ListRequest.ImgSizeEnum.Large;
            listRequest.Gl = "en";
            listRequest.Hl = "vi";
            try
            {
                var search = listRequest.Execute();
                return search;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
