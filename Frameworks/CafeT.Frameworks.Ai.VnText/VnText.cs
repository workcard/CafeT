using Google.Apis.Services;
using Google.Apis.Translate.v2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationsResource = Google.Apis.Translate.v2.Data.TranslationsResource;


namespace CafeT.Frameworks.Ai.VnText
{
    [Description("input")]
    public class TranslateInput
    {
        [Description("text to translate")]
        public string SourceText = "Who ate my candy?";
        [Description("target language")]
        public string TargetLanguage = "vi"; //fr - France, vi - Vietnamese
    }

    public static class VnText
    {
        public static string ToEnglish(this string text)
        {
            var key = new GoogleServices.GoogleServices().GetGoogleApiKey();
            TranslateInput input = new TranslateInput();
            input.SourceText = text;
            input.TargetLanguage = "en";

            // Create the service.
            var service = new TranslateService(new BaseClientService.Initializer()
            {
                ApiKey = key,
                ApplicationName = "MathBot"
            });

            string[] srcText = new[] { input.SourceText };
            var response = service.Translations.List(srcText, input.TargetLanguage).Execute();
            var translations = new List<string>();

            foreach (TranslationsResource translation in response.Translations)
            {
                translations.Add(translation.TranslatedText);
            }

            return translations.FirstOrDefault();
        }
        public static string ToVietnamese(this string text)
        {
            var key = new GoogleServices.GoogleServices().GetGoogleApiKey();
            TranslateInput input = new TranslateInput();
            input.SourceText = text;
            input.TargetLanguage = "vi";

            // Create the service.
            var service = new TranslateService(new BaseClientService.Initializer()
            {
                ApiKey = key,
                ApplicationName = "MathBot"
            });

            string[] srcText = new[] { input.SourceText };
            var response = service.Translations.List(srcText, input.TargetLanguage).Execute();
            var translations = new List<string>();

            foreach (TranslationsResource translation in response.Translations)
            {
                translations.Add(translation.TranslatedText);
            }

            return translations.FirstOrDefault();
        }
        //public static async Task<string[]> ToEnglishAsync(this string[] text)
        //{
        //    var key = new GoogleServices.GoogleServices().GetGoogleApiKey();
        //    TranslateInput input = new TranslateInput();
        //    input.SourceText = text;
        //    input.TargetLanguage = "en";

        //    // Create the service.
        //    var service = new TranslateService(new BaseClientService.Initializer()
        //    {
        //        ApiKey = key,
        //        ApplicationName = "MathBot"
        //    });

        //    string[] srcText = new[] { input.SourceText };
        //    var response = await service.Translations.List(srcText, input.TargetLanguage).ExecuteAsync();
        //    var translations = new List<string>();

        //    foreach (TranslationsResource translation in response.Translations)
        //    {
        //        translations.Add(translation.TranslatedText);
        //    }

        //    return translations.ToArray();
        //}
        /// <summary>User input for this example.</summary>


        //private async Task BotTranslate(IDialogContext context, IMessageActivity message)
        //{
        //    var key = new GoogleServices().GetGoogleApiKey();
        //    string _text = message.Text.Replace("#Translate", "").Replace("#Trans", "");

        //    TranslateInput input = new TranslateInput();
        //    input.SourceText = _text;
        //    input.TargetLanguage = "vi";

        //    // Create the service.
        //    var service = new TranslateService(new BaseClientService.Initializer()
        //    {
        //        ApiKey = key,
        //        ApplicationName = "MathBot"
        //    });

        //    string[] srcText = new[] { input.SourceText };
        //    var response = await service.Translations.List(srcText, input.TargetLanguage).ExecuteAsync();
        //    var translations = new List<string>();

        //    foreach (TranslationsResource translation in response.Translations)
        //    {
        //        await BotTalk(context, translation.TranslatedText);
        //    }

        //    return;
        //}
    }
}
