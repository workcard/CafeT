using CafeT.Objects;
using Google.Apis.Services;
using Google.Apis.Translate.v2;
using Google.Apis.Translate.v2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.GoogleManager
{
    public class Translator
    {
        private string CurrentApiKey = "AIzaSyCkwoW9Srl9ntpotdLNae2TWe2Wi98YFl8";
        private string CurrentApplicationName = "WorkCard.vn";
        private string TargetLangCode = "vi";
        private string SourceLangCode = "en";
        protected TranslateService TranService;
        //public Translator()
        //{
        //    TranService = new TranslateService(new BaseClientService.Initializer()
        //    {
        //        ApiKey = CurrentApiKey,
        //        ApplicationName = CurrentApplicationName
        //    });
        //}

        #region User Input
        /// <summary>User input for this example.</summary>
        [Description("input")]
        public class TranslateInput
        {
            [Description("text to translate")]
            public string SourceText = "en";
            [Description("target language")]
            public string TargetLanguage = "vi"; //fr - France, vi - Vietnamese
        }
        #endregion
        public void SetLanguage(string word)
        {
            Word _word = new Word(word);
            if (_word.Lang == WordLang.Vietnamese)
            {
                TargetLangCode = "en";
                SourceLangCode = "vi";
            }
        }
        //public string GetGoogleApiKey()
        //{
        //    return "AIzaSyCkwoW9Srl9ntpotdLNae2TWe2Wi98YFl8";
        //}
        //public string GetGoogleSearchEngine()
        //{
        //    return "004317969426278842680:_f5wbsgg7xc";
        //}

        public string Translate(string text, string targetLang)
        {
            var svc = new TranslateService(new BaseClientService.Initializer
            {
                ApiKey = CurrentApiKey,
                ApplicationName = CurrentApplicationName
            });
            SetLanguage(text);
            var listRequest = svc.Translations.List(text, targetLang);
            //listRequest.Key = "AIzaSyDz0FsFxxf7xsskqxlhaNWMvxM05b4HQBc";

            TranslationsListResponse response = listRequest.Execute();

            return response.Translations.Select(t => t.TranslatedText).ToArray()[0];
        }
    }
}
