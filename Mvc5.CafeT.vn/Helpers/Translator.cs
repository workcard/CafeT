using Google.Apis.Services;
using Google.Apis.Translate.v2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationsResource = Google.Apis.Translate.v2.Data.TranslationsResource;

public class Translator
{
    #region User Input
    /// <summary>User input for this example.</summary>
    [Description("input")]
    public class TranslateInput
    {
        [Description("text to translate")]
        public string SourceText = string.Empty;
        [Description("target language")]
        public string TargetLanguage = "en"; //fr - France, vi - Vietnamese
    }
    #endregion

    public List<string> Trans(string[] words)
    {
        List<string> _results = new List<string>();
        TranslateInput input = new TranslateInput();
        input.TargetLanguage = "vi";
        var service = new TranslateService(new BaseClientService.Initializer()
        {
            ApiKey = "AIzaSyA5naCjbPLqFdhaY6-f24bwoTwKSTHS9m4",
            ApplicationName = "CafeT"
        });

        string[] srcText = words.Distinct().ToArray();
        srcText = srcText.Take(100).ToArray();
        try
        {
            var response = service.Translations.List(srcText, input.TargetLanguage).Execute();
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


