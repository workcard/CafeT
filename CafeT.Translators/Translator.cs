using CafeT.Enumerable;
using CafeT.Objects;
using CafeT.Text;
using Google.Apis.Services;
using Google.Apis.Translate.v2;
using Google.Apis.Translate.v2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TranslationsResource = Google.Apis.Translate.v2.Data.TranslationsResource;

namespace CafeT.Translators
{
    public class Translator
    {
        private string CurrentApiKey = "AIzaSyA5naCjbPLqFdhaY6-f24bwoTwKSTHS9m4";
        private string CurrentApplicationName = "CafeT.vn";
        public string TargetLangCode = "vi";
        public string SourceLangCode = "en";
        protected TranslateService TranService;

        public Translator()
        {
            TranService = new TranslateService(new BaseClientService.Initializer()
            {
                ApiKey = CurrentApiKey,
                ApplicationName = CurrentApplicationName
            });
        }

        #region User Input
        [Description("input")]
        public class TranslateInput
        {
            [Description("text to translate")]
            public string SourceText = "en";
            [Description("target language")]
            public string TargetLanguage = "vi";
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
        public string GetGoogleApiKey()
        {
            return "AIzaSyDz0FsFxxf7xsskqxlhaNWMvxM05b4HQBc";
        }

        public string GetGoogleSearchEngine()
        {
            return "004317969426278842680:_f5wbsgg7xc";
        }

        public string Translate(string text, string source, string dest)
        {
            var listRequest = TranService.Translations.List(text, dest);
            listRequest.Source = source;
            
            TranslationsListResponse response = listRequest.Execute();
            
            string result = response.Translations
                .Select(t => t.TranslatedText)
                .ToArray()[0];

            return result;
        }

        public string Translate(string text)
        {
            var svc = new TranslateService(new BaseClientService.Initializer
            {
                ApiKey = GetGoogleApiKey()
            });
            SetLanguage(text);
            var listRequest = svc.Translations.List(text, TargetLangCode);
            listRequest.Key = "AIzaSyDz0FsFxxf7xsskqxlhaNWMvxM05b4HQBc";
            TranslationsListResponse response = listRequest.Execute();
            return response.Translations.Select(t=>t.TranslatedText).ToArray()[0];
        }

        public string Trans(string word)
        {
            List<string> _results = new List<string>();
            TranslateInput input = new TranslateInput();
            
            string[] srcText = new string[] { word };
            SetLanguage(word);
            input.TargetLanguage = TargetLangCode;
            input.SourceText = SourceLangCode;

            try
            {
                var response = TranService.Translations.List(srcText, input.TargetLanguage).Execute();
                foreach (TranslationsResource translation in response.Translations)
                {
                    if (translation.DetectedSourceLanguage == SourceLangCode)
                    {
                        _results.Add(translation.TranslatedText);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if(_results.Count > 0)
            {
                return _results.ElementAtOrDefault(0);
            }
            else
            {
                return string.Empty;
            }
        }
        public List<string> Trans(string[] words)
        {
            List<string> _results = new List<string>();
            TranslateInput input = new TranslateInput();
            input.TargetLanguage = TargetLangCode;
            input.SourceText = SourceLangCode;
            
            string[] srcText = words.Distinct().ToArray();
            srcText = srcText.TakeMax(100).ToArray();
            try
            {
                var response = TranService.Translations.List(srcText, input.TargetLanguage).Execute();
                foreach (TranslationsResource translation in response.Translations)
                {
                    _results.Add(translation.TranslatedText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return _results;
        }
    }
}
