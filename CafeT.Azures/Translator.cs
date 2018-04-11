using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Azures
{
    public static class AzureTranslator
    {
        /// Demonstrates getting an access token and using the token to translate.
        /// 
        //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com
        private const string SubscriptionKey = "3c45b3b12bda4d568939ab4fc7245944";   
        
        public static async Task<string> TranslateAsync(string text)
        {
            var translatorService = new TranslatorService.LanguageServiceClient();
            var authTokenSource = new AzureAuthToken(SubscriptionKey);
            var token = string.Empty;

            try
            {
                token = await authTokenSource.GetAccessTokenAsync();
            }
            catch (HttpRequestException)
            {
                switch (authTokenSource.RequestStatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Console.WriteLine("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
                        break;
                    case HttpStatusCode.Forbidden:
                        Console.WriteLine("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
                        break;
                }
                throw;
            }
            if(text.Contains("?vn"))
            {
                return translatorService.Translate(token, text, "en", "vi", "text/plain", "general", string.Empty);
            }
            else
            {
                return translatorService.Translate(token, text, "vi", "en", "text/plain", "general", string.Empty);
            }
        }
    }
}
