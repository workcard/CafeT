using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.ComponentModel;
using Google.Apis.Translate.v2;
using TranslationsResource = Google.Apis.Translate.v2.Data.TranslationsResource;
using System.Collections.Generic;
using Google.Apis.Services;
using CafeT.Text;
using System.Linq;
using CafeT.Objects;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using TiTiBot.Messages;
using CafeT.GoogleServices;
using AutoMapper;
using System.Text;
using System.Net.Sockets;
using CafeT.Html;
using System.Threading;
using TiTiBot.Services;
using System.Net.Http;
using TiTiBot.Users;

namespace TiTiBot.Dialogs
{

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        string strBaseURL;
        protected int intNumberToGuess;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("This is RootDialog");
            context.Wait(MessageReceivedAsync);
        }
        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            // Set BaseURL
            context.UserData.TryGetValue<string>("CurrentBaseURL", out strBaseURL);

            //var activity = await result as Activity;
            var activity = await result as Activity;
            string message = activity.Text;
            string lowerMessage = activity.Text.ToLower();
            
            if (lowerMessage.StartsWith("#>"))
            {
                Guest _guest = new Guest(context, lowerMessage);
                QnABot _bot = new QnABot();
                await _bot.AnswerAsync(_guest);
                //await _guest.ExcuteAsync();
            }
            else
            {
                QnABot _bot = new QnABot(context, message);
                if (lowerMessage == "#where")
                {
                    await _bot.TalkAsync(activity.AsMessageActivity().PrintAllProperties());
                }
                //else if (lowerMessage.StartsWith("#Translate".ToLower())
                //    || lowerMessage.StartsWith("#Trans".ToLower()))
                //{
                //    await _bot.Talk(activity.AsMessageActivity());
                //}
                else if (lowerMessage.StartsWith("#Update Email".ToLower()))
                {
                    await context.Forward(
                        new ProfileDialog(),
                        this.AfterGameDialogCompleted,
                        message,
                        CancellationToken.None);
                }
                //else if (lowerMessage.StartsWith("#Search".ToLower()))
                //{
                //    string _keyword = activity.Text.Replace("#Search", "");
                //    var _result = new GoogleServices().Search(_keyword);
                //    foreach (var item in _result.Items)
                //    {
                //        if (!item.Title.IsNullOrEmptyOrWhiteSpace())
                //            await BotTalk(context, item.Title);
                //        if (!item.Link.IsNullOrEmptyOrWhiteSpace())
                //            await BotTalk(context, item.Link);
                //    }
                //}
                else if (lowerMessage.StartsWith("#USD->VND".ToLower()))
                {
                    string _amountStr = activity.Text.Replace("#USD->VND ", "");
                    decimal _amount = decimal.Parse(_amountStr);
                    string _value = FinanceService.CurrencyConvert(_amount, "USD", "VND");

                    await _bot.TalkAsync(_value);
                }
                else if (lowerMessage.StartsWith("#Users".ToLower()))
                {
                    using (Models.TiTiBotDataContext dataContext = new Models.TiTiBotDataContext())
                    {
                        var _users = dataContext.Users;
                        foreach (var _user in _users)
                        {
                            await _bot.TalkAsync(_user.UserName);
                        }
                    }
                }
                //else if (lowerMessage.StartsWith("#View Profile".ToLower()))
                //{
                //    using (Models.TiTiBotDataContext dataContext = new Models.TiTiBotDataContext())
                //    {
                //        var newActivity = Mapper.Map<IMessageActivity, Models.ActivityBo>(activity);
                //        var _user = dataContext.Users.Where(t => t.UserName == newActivity.FromName).FirstOrDefault();
                //        if (_user != null)
                //        {
                //            try
                //            {
                //                await BotTalk(context, _user.PrintAllProperties());
                //                await BotTalk(context, newActivity.PrintAllProperties());
                //            }
                //            catch (Exception ex)
                //            {
                //                await BotTalk(context, ex.Message);
                //            }
                //        }
                //    }
                //}
                //http://vietlott.vn/vi/trung-thuong/ket-qua-trung-thuong/mega-6-45/winning-numbers/
                //else if (lowerMessage.StartsWith("#vietlott"))
                //{
                //    string _url = "http://vietlott.vn/vi/trung-thuong/ket-qua-trung-thuong/mega-6-45/winning-numbers/";
                //    await BotTalk(context, "Now i'm reading from (url): " + _url);
                //    WebPage _page = new WebPage(_url.Trim());
                //    if (_page.Rows != null && _page.Rows.Count > 0)
                //    {
                //        var _printRows = _page.Rows.Take(6).ToArray();
                //        for(int i = 1; i <= 6; i++)
                //        {
                //            await BotTalk(context, _printRows[i]);
                //        }
                //    }
                //}
                //else if (lowerMessage.StartsWith("#read") && lowerMessage.IsContainsUrl())
                //{
                //    UrlMessage _message = new UrlMessage(lowerMessage);
                //    _message.context = context;

                //    if (_message == null)
                //    {
                //        await BotTalkFinished(context);
                //    }
                //    await BotTalkStart(context, _message);
                //    await _message.ExcuteAsync();
                //    await BotTalkEnd(context, _message);
                //}
                //else if (lowerMessage.IsMathExpression())
                //{
                //    var _expr = MathEngine.ToMathExpr(lowerMessage);
                //    var _result = MathEngine.GetResult(_expr);
                //    //await BotTalk(context, _result);
                //}
                //else if (lowerMessage.StartsWith("#HotGirl".ToLower()))
                //{
                //    await context.Forward(
                //        new GameDialog(),
                //        this.AfterGameDialogCompleted,
                //        message,
                //        CancellationToken.None);
                //}
                else
                {
                    await context.Forward(
                        new QnADialog(),
                        this.ResumeAfterDoctorDialog,
                        message,
                        CancellationToken.None);
                    //context.Wait(MessageReceivedAsync);
                }
            }
            
        }

        #region private static List<CardAction> CreateButtons()
        private static List<CardAction> CreateButtons()
        {
            // Create 5 CardAction buttons 
            // and return to the calling method 
            List<CardAction> cardButtons = new List<CardAction>();
            for (int i = 1; i < 6; i++)
            {
                string CurrentNumber = Convert.ToString(i);
                CardAction CardButton = new CardAction()
                {
                    Type = "imBack",
                    Title = CurrentNumber,
                    Value = CurrentNumber
                };

                cardButtons.Add(CardButton);
            }

            return cardButtons;
        }
        #endregion
        #region private static Activity ShowButtons(IDialogContext context, string strText)
        private static Activity ShowButtons(IDialogContext context, string strText)
        {
            // Create a reply Activity
            Activity replyToConversation = (Activity)context.MakeMessage();
            replyToConversation.Text = strText;
            replyToConversation.Recipient = replyToConversation.Recipient;
            replyToConversation.Type = "message";

            // Call the CreateButtons utility method 
            // that will create 5 buttons to put on the Here Card
            List<CardAction> cardButtons = CreateButtons();

            // Create a Hero Card and add the buttons 
            HeroCard plCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            // Create an Attachment
            // set the AttachmentLayout as 'list'
            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);
            replyToConversation.AttachmentLayout = "list";

            // Return the reply to the calling method
            return replyToConversation;
        }
        #endregion

        public async Task BotTalkFinished(IDialogContext context)
        {
            await context.PostAsync("Finished. Pls continue chat with me");
            context.Wait(MessageReceivedAsync);
        }

        //public async Task BotTalkStart(IDialogContext context, UrlMessage message)
        //{
        //    await context.PostAsync("Now, i'm starting process with your request");
        //    await context.PostAsync(message.Command + ":" + message.Urls[0]);
        //}
        //public async Task BotTalkEnd(IDialogContext context, UrlMessage message)
        //{
        //    await context.PostAsync(message.Command + ":" + message.Urls[0]);
        //    await context.PostAsync("Finished. Pls continue chat with me");
        //}
        //public async Task BotTalk(IDialogContext context, string msg)
        //{
        //    IBotMessage message;
        //    if(msg.IsContainsUrl())
        //    {
        //        message = new UrlMessage(msg);
        //    }
        //    else
        //    {
        //        TextMessage _message = new TextMessage();
        //        _message.Title = string.Empty;
        //        _message.Content = msg;
        //        await context.PostAsync(msg);
        //        await context.PostAsync("Finished. Pls continue chat with me");
        //    }
        //}
    
        /// <summary>User input for this example.</summary>
        [Description("input")]
        public class TranslateInput
        {
            [Description("text to translate")]
            public string SourceText = "Who ate my candy?";
            [Description("target language")]
            public string TargetLanguage = "vi"; //fr - France, vi - Vietnamese
        }

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

        #region Helper Methods
        void Prompt(IDialogContext context)
        {
            try
            {
                // Prompt Choice
                PromptDialog.Choice<string>(
                    context: context,
                    resume: SelectedOption,
                    options: RoutingOption,
                    prompt: "Hello! How may I help you with?",
                    retry: "Sorry, I did not understand what you mean by that.",
                    attempts: 3,
                    promptStyle: PromptStyle.Keyboard);
            }
            catch (TooManyAttemptsException)
            {
                // Log exception
            }
        }

        private async Task SelectedOption(IDialogContext context, IAwaitable<string> result)
        {
            string selectedOption = await result;
            switch (selectedOption)
            {
                case "Doctor":
                    context.Call(new DoctorDialog(), this.ResumeAfterDoctorDialog);
                    break;
                case "Broadband":
                    await CallBroadbandDialog(context, selectedOption);
                    break;
                default:
                    break;
            }
        }

        Task CallBroadbandDialog(IDialogContext context, string text)
        {
            var message = context.MakeMessage();
            message.Text = text;

            return context.Forward(new BroadbandDialog(), this.ResumeAfterBroadbandDialog, message, CancellationToken.None);
        }
        private IEnumerable<string> RoutingOption = new List<string> { "Doctor", "Broadband" };
        #endregion

        #region ResumeAfter
        private Task ResumeAfterDoctorDialog(IDialogContext context, IAwaitable<object> result)
        {
            return Task.CompletedTask;
        }

        private Task ResumeAfterBroadbandDialog(IDialogContext context, IAwaitable<object> result)
        {
            return Task.CompletedTask;
        }
        private async Task AfterGameDialogCompleted(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
        private async Task AfterHelloCompleted(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
        private async Task ResumeAfterNewOrderDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task ResumeAfterLuisDialog(IDialogContext context, IAwaitable<object> result)
        {

            string message = $"Now, i'm using LUIS ";
            string _result = await FinanceService.GetEntityFromLUIS("one person in this hotel");
            await context.PostAsync(message);
            await context.PostAsync(_result);

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task AfterCommonResponseHandled(IDialogContext context, IAwaitable<object> result)
        {
            var messageHandled = await result as Activity;
            bool isContinue = false;
            if (bool.TryParse(messageHandled.Text, out isContinue))
            {
                if (!isContinue)
                {
                    await context.PostAsync("I’m not sure what you want.");
                }
            }
            context.Wait(MessageReceivedAsync);
        }

        private async Task ResumeAfterWelcome(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }
        private async Task ResumeAfterWelcomeDone(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
        #endregion
    }
}