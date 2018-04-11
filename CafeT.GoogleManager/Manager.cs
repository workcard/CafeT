using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using Google.Apis.Translate.v2;
using Google.Apis.Translate.v2.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CafeT.GoogleManager
{
    public interface IManager
    {
    }

    //public class Translator
    //{
    //    private string CurrentApiKey = "AIzaSyA5naCjbPLqFdhaY6-f24bwoTwKSTHS9m4";
    //    private string CurrentApplicationName = "CafeT.vn";
    //    private string TargetLangCode = "vi";
    //    private string SourceLangCode = "en";
    //    protected TranslateService TranService;
    //    public Translator()
    //    {
    //        TranService = new TranslateService(new BaseClientService.Initializer()
    //        {
    //            ApiKey = CurrentApiKey,
    //            ApplicationName = CurrentApplicationName
    //        });
    //    }

    //    #region User Input
    //    /// <summary>User input for this example.</summary>
    //    [Description("input")]
    //    public class TranslateInput
    //    {
    //        [Description("text to translate")]
    //        public string SourceText = "en";
    //        [Description("target language")]
    //        public string TargetLanguage = "vi"; //fr - France, vi - Vietnamese
    //    }
    //    #endregion
    //    public void SetLanguage(string word)
    //    {
    //        Word _word = new Word(word);
    //        if (_word.Lang == WordLang.Vietnamese)
    //        {
    //            TargetLangCode = "en";
    //            SourceLangCode = "vi";
    //        }
    //    }
    //    public string GetGoogleApiKey()
    //    {
    //        return "AIzaSyDz0FsFxxf7xsskqxlhaNWMvxM05b4HQBc";
    //    }
    //    public string GetGoogleSearchEngine()
    //    {
    //        return "004317969426278842680:_f5wbsgg7xc";
    //    }

    //    public string Translate(string text, string targetLang)
    //    {
    //        var svc = new TranslateService(new BaseClientService.Initializer
    //        {
    //            ApiKey = GetGoogleApiKey()
    //        });
    //        //SetLanguage(text);
    //        var listRequest = svc.Translations.List(text, targetLang);
    //        listRequest.Key = "AIzaSyDz0FsFxxf7xsskqxlhaNWMvxM05b4HQBc";
        
    //        TranslationsListResponse response = listRequest.Execute();

    //        return response.Translations.Select(t => t.TranslatedText).ToArray()[0];
    //    }
    //}

    public class Manager
    {
        private string Key { set; get; }
        private string EngineKey { set; get; }
        private string TargetLangCode = "vi";
        private string SourceLangCode = "en";

        public Manager(string apiKey, string searchEngineKey)
        {
            Key = apiKey;
            EngineKey = searchEngineKey;
        }
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

        private Google.Apis.Translate.v2.TranslationsResource.ListRequest MakeTranlationsRequest(string keywords)
        {
            var svc = new TranslateService(new BaseClientService.Initializer
            {
                ApiKey = Key
            });

            var listRequest = svc.Translations.List(keywords,TargetLangCode);
            listRequest.Key = EngineKey;
            return listRequest;
        }

        public string Translate(string text, string targetLang)
        {
            var request = MakeTranlationsRequest(text);
            TranslationsListResponse response = request.Execute();
            return response.Translations.Select(t => t.TranslatedText).ToArray()[0];
        }

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
