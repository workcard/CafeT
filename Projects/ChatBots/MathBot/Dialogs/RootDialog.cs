using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathBot.Models;
using CafeT.Text;
using System.Net.Http;
using CafeT.Translators;
using CafeT.GoogleServices;
using CafeT.Enumerable;
using CafeT.Mathematics;
using CafeT.Html;
using CafeT.Convertors;

namespace MathBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            await ProcessMessageAsync(context, message);
        }
        public async Task ProcessMessageAsync(IDialogContext context, IMessageActivity message)
        {
            UserMessage userText = new UserMessage(message.Text);
            if(userText.Type == UserMessageType.Url)
            {
                UrlModel url = new UrlModel(userText.Text);
                if(url.Ping())
                {
                    using (MathBotDataContext dbContext = new MathBotDataContext())
                    {
                        if(!dbContext.Urls.Select(t=>t.Url).Contains(url.Url))
                        {
                            try
                            {
                                dbContext.Urls.Add(url);
                                await dbContext.SaveChangesAsync();
                                await BotTalk(context, MakeBotMessage(userText));
                            }
                            catch (Exception ex)
                            {
                                await BotTalk(context, ex.Message);
                            }
                        }
                        else
                        {
                            await BotTalk(context, url.Url + " is exits");
                        }
                    }
                }
                return;
            }
            else if (userText.Type == UserMessageType.MathExpression)
            {
                MathEngine mathEngine = new MathEngine();
                string _result = mathEngine.Calc(userText.Text);
                await BotTalk(context, _result);
                return;
            }
            else if(userText.Type == UserMessageType.DatabaseCommand)
            {
                string command = "";
                string sql = userText.Text.GetFromEndTo("#");
                string table = userText.Text.NextWords("#")[0];
                using (MathBotDataContext dbContext = new MathBotDataContext())
                {
                    if(table == "Url")
                    {
                        string lastUrl = dbContext.Urls
                            .OrderByDescending(t=>t.CreatedDate)
                            .FirstOrDefault().Url;
                        await BotTalk(context, lastUrl);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if(userText.Type == UserMessageType.Translate)
            {
                await BotTranslate(context, userText);
                return;
            }
            else if (userText.Type == UserMessageType.ChangeCurrency)
            {
                if(userText.Text.ToLower().EndsWith("vnd"))
                {
                    string amount = userText.Text.GetFromBeginTo("?").Trim();
                    decimal _amount = decimal.Parse(amount);
                    string endToken = userText.Text.GetFromEndTo("?").Trim();
                    var results = Convertor.CurrencyConvert(_amount, "USD", "VND");
                    string replyMessage = results;
                    await BotTalk(context, replyMessage);
                }
                return;
            }
            else if (userText.Text.StartsWith("#vietlott"))
            {
                string _url = "http://vietlott.vn/vi/trung-thuong/ket-qua-trung-thuong/";
                await BotTalk(context, "Now i'm reading from (url): " + _url);
                WebPage _page = new WebPage(_url.Trim());
                if (_page.HtmlTables != null && _page.HtmlTables.Count > 0)
                {
                    foreach (var table in _page.HtmlTables)
                    {
                        foreach(var row in table.Rows)
                        {
                            if(row.IsContainsNumber())
                            {
                                string text = row.ToHtmlDocument().DocumentNode.InnerText;
                                var numbers = text.GetNumbers()
                                    .WordsToString();
                                if (!numbers.IsNullOrEmptyOrWhiteSpace())
                                {
                                    await BotTalk(context, numbers);
                                }
                            }
                        }
                    }
                }
            }
            else if (userText.Type == UserMessageType.Search)
            {
                string command = userText.Text;
                var results = new GoogleServices()
                    .Search(command);
                var items = results.Items.TakeMax(5);
                foreach(var item in results.Items)
                {
                    await BotTalk(context, item.HtmlSnippet);
                }
                return;
            }
            else if (userText.Type == UserMessageType.ImageSearch)
            {
                string command = userText.Text;
                var results = new GoogleServices()
                    .SearchImage(command);
                var items = results.Items.TakeMax(5);
                foreach (var item in results.Items)
                {
                    await BotTalk(context, item.HtmlSnippet);
                }
                return;
            }
            else if (userText.Type == UserMessageType.SayGoodbye)
            {
                await BotTalk(context, "Goodbye");
                return;
            }
            else if (userText.Type == UserMessageType.SayHello)
            {
                await BotTalk(context, "Hello");
                return;
            }
            else
            {
                await this.SendUnknowMessageAsync(context);
            }
        }

        private async Task SendUnknowMessageAsync(IDialogContext context)
        {
            await context.PostAsync("Sorry, I can not understand your message. Pls try agian");
            context.Call(new ThumbnailCardDialog(), this.NameDialogResumeAfter);
            return;
        }

        private async Task NameDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                await context.PostAsync("I'm NameDialogResumeAfter, I'm having issues understanding you. Let's try again.");
                return;
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                await this.SendUnknowMessageAsync(context);
            }
        }
        
       
        public string MakeBotMessage(UserMessage message)
        {
            string replyMessage = string.Empty;
            if (message.Type == UserMessageType.Url)
            {
                replyMessage = "This url saved in MathBot database";
                return replyMessage;
            }
            return replyMessage;
        }
       
        public string SearchInDictionary(string text)
        {
            string _word = text.ToLower();
            using (MathBotDataContext dbContext = new MathBotDataContext())
            {
                var _result = dbContext.Dictionaries.Where(t => t.English.ToLower().Contains(_word));
                if(_result != null)
                {
                    return dbContext.Dictionaries
                        .Where(t => t.English.ToLower().Contains(_word))
                        .Select(t => t.Vietnameses)
                        .FirstOrDefault();
                }
                else if(dbContext.Dictionaries.Where(t => t.Vietnameses.ToLower().Contains(_word)) != null)
                {
                    return dbContext.Dictionaries
                        .Where(t => t.Vietnameses.ToLower().Contains(_word))
                        .Select(t => t.English)
                        .FirstOrDefault();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private async Task BotTranslate(IDialogContext context, UserMessage userText)
        {
            bool isSave = true;
            string _text = userText.Text.GetFromBeginTo("?").Trim();
            string _token = userText.Text.GetFromEndTo("?").Trim();
            string messageText = userText.Text.GetFromBeginTo("?").Trim();

            var _results = SearchInDictionary(_text);
            if(_results != null)
            {
                isSave = false;
                await context.PostAsync(_results);
                return;
            }
            else
            {
                Translator translator = new Translator();
                WordDictionary _newWord = new WordDictionary();
                _newWord.CountViews = 0;
                _newWord.IsRemembered = false;

                if(_token.IsEnglishLangCode())
                {
                    _newWord.English = _text;
                    _newWord.Vietnameses = translator.Translate(_text, "vi", "en");
                    _newWord.Vietnameses = System.Web.HttpUtility.HtmlDecode(_newWord.Vietnameses);
                    await context.PostAsync(_newWord.Vietnameses);
                }
                else if(_token.IsVietnameseLangCode())
                {
                    _newWord.Vietnameses = _text;
                    _newWord.English = translator.Translate(_text, "en", "vi");
                    _newWord.English = System.Web.HttpUtility.HtmlDecode(_newWord.English);
                    await context.PostAsync(_newWord.English);
                }
                else
                {
                    isSave = false;
                    await context.PostAsync("Sorry, i don't understand this language");
                    return;
                }

                if(isSave)
                {
                    using (MathBotDataContext dbContext = new MathBotDataContext())
                    {
                        dbContext.Dictionaries.Add(_newWord);
                        try
                        {
                            dbContext.SaveChanges();
                            return;
                        }
                        catch (Exception ex)
                        {
                            await context.PostAsync(ex.Message);
                            return;
                        }
                    }
                }
            }
        }

        public List<string> Search(string[] keywords)
        {
            List<string> _urlsResult = new List<string>();
            foreach (string _keyword in keywords)
            {
                using (MathBotDataContext dbContext = new MathBotDataContext())
                {
                    var _urls = dbContext.Urls.Where(t => t.Url.ToLower()
                                        .Contains(_keyword.ToLower()))
                                        .Select(t => t.Url).ToList();
                    _urlsResult.AddRange(_urls);
                }
            }
            return _urlsResult.Distinct().ToList();
        }

        public async Task BotTalkWithCard(IDialogContext context, string msg)
        {
            ThumbnailCardDialog dialog = new ThumbnailCardDialog();
            await dialog.StartAsync(context);
        }
        public async Task BotTalk(IDialogContext context, string msg)
        {
            await context.PostAsync($"{msg}");
            return;
        }
    }
}