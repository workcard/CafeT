using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using QnAMakerDialog;
using CafeT.Text;
using CafeT.Html;
using QNABot.Models;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Bot.Connector;

namespace TiTiBot.Dialogs
{
    [Serializable]
    [QnAMakerService("2402d81685d8435b95578cb62f5e4dec", "5a2229c4-7aae-4152-b895-69c196bffa76")]
    public class UpdateQnADialog : IDialog<object>
    {
        public string Question { set; get; }
        public UpdateQnADialog(string question)
        {
            Question = question;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Câu trả lời của bạn là: ?");
            context.Wait(this.MessageReceivedAsync);
        }

        // QNA Methods
        // Global values to hold the custom settings
        private static string _OcpApimSubscriptionKey = "2402d81685d8435b95578cb62f5e4dec";
        private static string _KnowledgeBase = "5a2229c4-7aae-4152-b895-69c196bffa76";

        enum Mode { Add, Delete };

        public async Task<bool> UpdateKbAsync(string question, string answer)
        {
            
            QnAQuery objQnAResult = new QnAQuery();
            objQnAResult.Message = "";
            try
            {
                string _newQ = question;
                string _newA = string.Empty;
                _newA = answer;
                objQnAResult.Message = await UpdateQueryQnABot(_newQ, _newA, Mode.Add);
                objQnAResult.Message = await TrainAndPublish();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> argument)
        {
            var activity = await argument as Activity;
            string message = activity.Text;
            Question = "{" + context.GetCurrentUser().Email + "}" + Question;
            message = "{" + context.GetCurrentUser().Email + "}" + message;
            var result = await UpdateKbAsync(Question, message);
            if (result)
            {
                await context.PostAsync($"Đã cập nhật kiến thức mới");
            }
            else
            {
                await context.PostAsync($"Có lỗi xảy ra. Không cập nhật được kiến thức");
            }
            context.Done("Finish update KB.");
        }

        #region private static async Task<QnAQuery> QueryQnABot(string Query)
        private static async Task<QnAQuery> QueryQnABot(string Query)
        {
            QnAQuery QnAQueryResult = new QnAQuery();

            using (System.Net.Http.HttpClient client =
                new System.Net.Http.HttpClient())
            {
                string RequestURI = String.Format("{0}{1}{2}{3}{4}",
                    @"https://westus.api.cognitive.microsoft.com/",
                    @"qnamaker/v1.0/",
                    @"knowledgebases/",
                    _KnowledgeBase,
                    @"/generateAnswer");

                var httpContent =
                    new StringContent($"{{\"question\": \"{Query}\"}}",
                    Encoding.UTF8, "application/json");

                httpContent.Headers.Add(
                    "Ocp-Apim-Subscription-Key", _OcpApimSubscriptionKey);

                System.Net.Http.HttpResponseMessage msg =
                    await client.PostAsync(RequestURI, httpContent);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse =
                        await msg.Content.ReadAsStringAsync();

                    QnAQueryResult =
                        JsonConvert.DeserializeObject<QnAQuery>(JsonDataResponse);
                }
            }
            return QnAQueryResult;
        }
        #endregion

        #region private static async Task<string> GetFAQ()
        private static async Task<string> GetFAQ()
        {
            string strFAQUrl = "";
            string strLine;
            StringBuilder sb = new StringBuilder();

            // Get the URL to the FAQ (in .tsv form)
            using (System.Net.Http.HttpClient client =
                new System.Net.Http.HttpClient())
            {
                string RequestURI = String.Format("{0}{1}{2}{3}{4}",
                    @"https://westus.api.cognitive.microsoft.com/",
                    @"qnamaker/v2.0/",
                    @"knowledgebases/",
                    _KnowledgeBase,
                    @"? ");

                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", _OcpApimSubscriptionKey);

                System.Net.Http.HttpResponseMessage msg =
                    await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse =
                        await msg.Content.ReadAsStringAsync();

                    strFAQUrl =
                        JsonConvert.DeserializeObject<string>(JsonDataResponse);
                }
            }

            // Make a web call to get the contents of the
            // .tsv file that contains the database
            var req = WebRequest.Create(strFAQUrl);
            var r = await req.GetResponseAsync().ConfigureAwait(false);

            // Read the response
            using (var responseReader = new StreamReader(r.GetResponseStream()))
            {
                // Read through each line of the response
                while ((strLine = responseReader.ReadLine()) != null)
                {
                    // Write the contents to the StringBuilder object
                    string[] strCurrentLine = strLine.Split('\t');
                    sb.Append((String.Format("{0},{1},{2}\n",
                        CleanContent(strCurrentLine[0]),
                        CleanContent(strCurrentLine[1]),
                        CleanContent(strCurrentLine[2])
                        )));
                }
            }

            // Return the contents of the StringBuilder object
            return sb.ToString();
        }
        #endregion

        #region private static async Task<string> UpdateQueryQnABot(string newQuestion, string newAnswer, Mode paramMode)
        private static async Task<string> UpdateQueryQnABot(
            string newQuestion, string newAnswer, Mode paramMode)
        {
            string strResponse = "";

            // Create the QnAKnowledgeBase that contains the new entry
            QnAKnowledgeBase objQnAKnowledgeBase = new QnAKnowledgeBase();
            QnaPair objQnaPair = new QnaPair();
            objQnaPair.question = newQuestion;
            objQnaPair.answer = newAnswer;

            if (paramMode == Mode.Add)
            {
                Add objAdd = new Add();
                objAdd.qnaPairs = new List<QnaPair>();
                objAdd.urls = new List<string>();
                objAdd.urls.Add(@"https://azure.microsoft.com/en-us/support/faq/");
                objAdd.qnaPairs.Add(objQnaPair);
                objQnAKnowledgeBase.add = objAdd;
            }

            if (paramMode == Mode.Delete)
            {
                Delete objDelete = new Delete();
                objDelete.qnaPairs = new List<QnaPair>();
                objDelete.urls = new List<string>();
                objDelete.urls.Add(@"http://localhost");
                objDelete.qnaPairs.Add(objQnaPair);
                objQnAKnowledgeBase.delete = objDelete;
            }

            using (System.Net.Http.HttpClient client =
                new System.Net.Http.HttpClient())
            {
                string RequestURI = String.Format("{0}{1}{2}{3}? ",
                    @"https://westus.api.cognitive.microsoft.com/",
                    @"qnamaker/v2.0/",
                    @"knowledgebases/",
                    _KnowledgeBase);

                using (HttpRequestMessage request =
                    new HttpRequestMessage(new HttpMethod("PATCH"), RequestURI))
                {
                    request.Content = new StringContent(
                        JsonConvert.SerializeObject(objQnAKnowledgeBase),
                        System.Text.Encoding.UTF8, "application/json");

                    request.Content.Headers.Add(
                        "Ocp-Apim-Subscription-Key",
                        _OcpApimSubscriptionKey);

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        strResponse = $"Operation {paramMode} completed.";
                    }
                    else
                    {
                        string responseContent =
                            await response.Content.ReadAsStringAsync();

                        strResponse = responseContent;
                    }
                }
            }

            return strResponse;
        }
        #endregion

        #region private static async Task<string> TrainAndPublish()
        private static async Task<string> TrainAndPublish()
        {
            string strResponse = "";

            using (System.Net.Http.HttpClient client =
                new System.Net.Http.HttpClient())
            {
                string RequestURI = String.Format("{0}{1}{2}{3}",
                    @"https://westus.api.cognitive.microsoft.com/",
                    @"qnamaker/v2.0/",
                    @"knowledgebases/",
                    _KnowledgeBase);

                var httpContent =
                    new StringContent("",
                    Encoding.UTF8, "application/json");

                httpContent.Headers.Add(
                    "Ocp-Apim-Subscription-Key", _OcpApimSubscriptionKey);

                System.Net.Http.HttpResponseMessage response =
                    await client.PutAsync(RequestURI, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    strResponse = $"Operation Train and Publish completed.";
                }
                else
                {
                    string responseContent =
                        await response.Content.ReadAsStringAsync();

                    strResponse = responseContent;
                }
            }

            return strResponse;
        }
        #endregion

        // Utility

        #region private static string CleanContent(string paramString)
        private static string CleanContent(string paramString)
        {
            // Clean line breaks
            paramString = paramString.Replace(@"\n", "");

            // Clean commas
            paramString = paramString.Replace(",", "");

            return paramString;
        }

        
        #endregion
    }
}