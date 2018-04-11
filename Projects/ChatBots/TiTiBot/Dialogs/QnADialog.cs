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
using System.Threading;
using CafeT.BotMessages;
using Microsoft.Bot.Connector;
using CafeT.Frameworks.Ai.VnText;
using CafeT.GoogleServices;
using Newtonsoft.Json.Linq;
using CafeT.Bots;
using TiTiBot.Users;

namespace TiTiBot.Dialogs
{
    
    [Serializable]
    [QnAMakerService("2402d81685d8435b95578cb62f5e4dec", "5a2229c4-7aae-4152-b895-69c196bffa76")]
    public class QnADialog : QnAMakerDialog<object>
    {
        //protected string Message { set; get; }

        string strBaseURL;
        protected int intNumberToGuess;
        protected int intAttempts;
        protected string UnknowMessage = "Xin lỗi, Tôi chưa biết câu trả lời.";
        protected string HintMessage = "Bạn có muốn thêm câu trả lời (chính xác) cho câu hỏi này";
        protected string Message { set; get; } = string.Empty;

        
        protected IDialogContext Context;
        
        
        protected Guest User { set; get; }
        protected QnABot Bot { set; get; }

        
        public QnADialog()
        {
            User = new Guest();
            Bot = new QnABot();
        }
        //public QnADialog(string message)
        //{
        //    Message = message;
        //    Guest _guest = new Guest(Message);
        //    //activity.Text = message.ToEnglish();
        //    QnABot _bot = new QnABot(_guest.Context);
        //    _bot.AnswerAsync(_guest);
        //}
        //public string UnkownQuestion { set; get; } = string.Empty;
        /// <summary>
        /// Handler used when the QnAMaker finds no appropriate answer
        /// </summary>
        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            User.Context = context;
            User.Message = originalQueryText;
            await Bot.AnswerAsync(User);

            //Bot = new QnABot(context, originalQueryText);
            //await Bot.ExcuteAsync();
            //await Bot.SaveMessageAsync();

            //if (originalQueryText.IsUrl())
            //{
            //    QnABot _bot = new QnABot(context, originalQueryText);
            //    await _bot.SaveMessageAsync();
            //}
            //else
            //{
            //    await context.PostAsync($"{UnknowMessage} cho '{originalQueryText}'.");
            //    //if(originalQueryText.IsVietnamese())
            //    //{
            //    //    await context.PostAsync($"{UnknowMessage} cho '{originalQueryText}'.");
            //    //}
            //    try
            //    {
            //        string _english = originalQueryText.ToEnglish();
            //        await context.PostAsync($"{UnknowMessage} cho '{_english}'.");
            //    }
            //    catch (Exception ex)
            //    {
            //        await context.PostAsync($"{ex.Message}");
            //    }
            //    //try
            //    //{
            //    //    string _english = originalQueryText.ToVietnamese();
            //    //    await context.PostAsync($"{UnknowMessage} cho '{_english}'.");
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    await context.PostAsync($"{ex.Message}");
            //    //}

            //    //var _result = new GoogleServices().Search(originalQueryText);
            //    //if (_result != null && _result.Items != null && _result.Items.Count > 0)
            //    //{
            //    //    int _count = _result.Items.Count;
            //    //    if (_count > 5)
            //    //    {
            //    //        _count = 5;
            //    //    }
            //    //    for (int i = 0; i < _count; i++)
            //    //    {
            //    //        //if (!_result.Items[i].HtmlTitle.IsNullOrEmptyOrWhiteSpace())
            //    //        //    await context.PostAsync(WebUtility.HtmlDecode(_result.Items[i].HtmlTitle));
            //    //        //if (!_result.Items[i].HtmlSnippet.IsNullOrEmptyOrWhiteSpace())
            //    //        //    await context.PostAsync(WebUtility.HtmlDecode(_result.Items[i].HtmlSnippet));
            //    //        //if (!_result.Items[i].Title.IsNullOrEmptyOrWhiteSpace())
            //    //        //{
            //    //        //    await context.PostAsync(_result.Items[i].Title);
            //    //        //}
            //    //        if (!_result.Items[i].Link.IsNullOrEmptyOrWhiteSpace())
            //    //        {
            //    //            await context.PostAsync(_result.Items[i].Link);
            //    //        }
            //    //    }
            //    //}
            //    var _result = new MicrosoftServices().Search(originalQueryText);
            //    if (_result != null)
            //    {
            //        await context.PostAsync(WebUtility.HtmlDecode(_result.Result));
            //    }
            //    else
            //    {
            //        LastUserInput = originalQueryText;
            //        if (context.GetCurrentUser().HasEmail())
            //        {
            //            if (context.GetCurrentUser().HasVerified())
            //            {
            //                TalkWithPrompt(context, LastUserInput);
            //            }
            //            else
            //            {
            //                await context.PostAsync("Xin lỗi. Tài khoản của bạn chưa được xác nhận nên không thể cập nhật kiến thức mới được.");
            //            }
            //        }
            //    }
            //}
        }
        protected string LastUserInput = string.Empty;

        private async Task PlayAgainAsync(IDialogContext context, IAwaitable<bool> result)
        {
            var confirm = await result;

            if (confirm) // They said yes
            {
                var message = LastUserInput;
                var _childDialog = new UpdateQnADialog(message);
                await _childDialog.StartAsync(context);
            }
            else // They said no
            {
                await context.PostAsync("Bạn có cần gì nữa không ?");
            }
        }

        public void TalkWithPrompt(IDialogContext context, string text)
        {
            // Game completed
            StringBuilder sb = new StringBuilder();

            sb.Append("Hỏi: ");
            sb.Append($"{HintMessage} : {text}");

            string CongratulationsStringPrompt =
                string.Format(sb.ToString(),
                this.intNumberToGuess,
                this.intAttempts);

            // Put PromptDialog here
            PromptDialog.Confirm(
                context,
                PlayAgainAsync,
                CongratulationsStringPrompt,
                "Didn't get that!");
        }
       
        private async Task ResumeAfterDoctorDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }
        /// <summary>
        /// This is the default handler used if no specific applicable score handlers are found
        /// </summary>
        public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            // ProcessResultAndCreateMessageActivity will remove any attachment markup from the results answer
            // and add any attachments to a new message activity with the message activity text set by default
            // to the answer property from the result
            var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);
            messageActivity.Text = $"I found an answer that might help...{result.Answer}.";
            //User = new Guest();
            User.Context = context;
            User.Message = originalQueryText;

            Bot = new QnABot(context);
            await Bot.AnswerAsync(User);

            //await ProcessResult(context, result.Answer);
            context.Wait(MessageReceived);
        }

        
        /// <summary>
        /// Handler to respond when QnAMakerResult score is a maximum of 70
        /// </summary>
        //[QnAMakerResponseHandler(80)]
        //public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        //{
        //    var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);
        //    messageActivity.Text = $"I found an answer that might help...{result.Answer}.";

        //    User.Context = context;
        //    User.Message = result.Answer;

        //    await Bot.AnswerAsync(User);

        //    context.Wait(MessageReceived);
        //}
    }
}