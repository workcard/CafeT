using CafeT.BotMessages;
using CafeT.Bots;
using CafeT.Text;
using MathBot.Managers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TiTiBot.Models;
using TiTiBot.Users;

namespace TiTiBot
{
    [Serializable]
    public class QnABot:BaseBot
    {
        //[Serializable]
        //protected Activity ActivityObj { set; get; } = new Activity();
        protected string Message { set; get; } = string.Empty;
        protected string RequestMessage { set; get; } = string.Empty;
        public QnABot():base(){ }
        public QnABot(IDialogContext context) : base(context){ }

        public QnABot(IDialogContext context, string message) : base(context)
        {
            Message = message;
        }

        public async Task AnswerAsync(Guest guest)
        {
            Context = guest.Context;
            RequestMessage = guest.Message;
            await ExcuteAsync();
        }

        public async Task ProcessResult()
        {
            if (RequestMessage.IsYouTubeUrl())
            {
                IBotMessage _message = new VideoMessage(Context, RequestMessage);
                await _message.ExcuteAsync();
                return;
            }
            //else if (RequestMessage.IsCsharpCode())
            //{
            //    IBotMessage _message = new CsharpMessage(Context, RequestMessage);
            //    await _message.ExcuteAsync();
            //    return;
            //}
            else if (RequestMessage.IsUrl())
            {
                IBotMessage _message = new UrlMessage(Context, RequestMessage);
                await _message.ExcuteAsync();
                return;
            }
            else if (RequestMessage.IsContainsUrl())
            {
                string _url = RequestMessage.GetUrls()[0];
                IBotMessage _message = new TextMessage(Context, RequestMessage);
                await _message.ExcuteAsync();
                return;
            }
            else //TextMessage
            {
                IBotMessage _message = new TextMessage(Context, RequestMessage);
                await _message.ExcuteAsync();
                return;
            }
        }

        public async Task ExcuteAsync()
        {
            if (RequestMessage.ToLower().Contains("#>Urls".ToLower()))
            {
                UrlManager _manager = new UrlManager();
                var _urls = _manager.GetAllUrls().Select(t => t.Address);
                if (_urls != null && _urls.Count() > 0)
                {
                    foreach (string url in _urls)
                    {
                        await TalkAsync(url);
                    }
                }
            }
            else if (RequestMessage.ToLower().Contains("#>Users".ToLower()))
            {
                UserManager _manager = new UserManager();
                var _urls = _manager.GetAll().Select(t => t.UserName);
                if (_urls != null && _urls.Count() > 0)
                {
                    foreach (string url in _urls)
                    {
                        await TalkAsync(url);
                    }
                }
            }
            else
            {
                await ProcessResult();
            }
        }

        //public async Task StartAsync(Activity activity)
        //{
        //    ActivityObj = activity;
        //    Message = ActivityObj.Text;
        //    if(Message.StartsWith("#>"))
        //    {
        //        await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
        //    }
        //    else
        //    {
        //        await Conversation.SendAsync(activity, () => new Dialogs.QnADialog());
        //    }
        //}

        public async Task SaveMessageAsync()
        {
            if(RequestMessage.IsUrl())
            {
                Url _url = new Url(Message);
                UrlManager _manager = new UrlManager();
                string notify = await _manager.AddUrlAsync(_url);
                await this.TalkAsync(notify);
            }
        }
    }
}