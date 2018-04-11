using MathBot.Managers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TiTiBot.Users
{
    public abstract class BaseUser
    {
        public IDialogContext Context { set; get; }
        public string UserName { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public async Task TalkAsync(string message)
        {
            await Context.PostAsync($"{message}");
        }
    }

    [Serializable]
    public class Guest:BaseUser
    {
        
        
        public string Message { set; get; } = string.Empty;
        public Guest() { }
        public Guest(IDialogContext context)
        {
            Context = context;
        }
        public Guest(IDialogContext context, string message)
        {
            Context = context;
            Message = message;
        }
        public Guest(string message)
        {
            Message = message;
        }
        //public async Task ExcuteAsync()
        //{
        //    if (Message.ToLower().Contains("#>Urls".ToLower()))
        //    {
        //        UrlManager _manager = new UrlManager();
        //        var _urls = _manager.GetAllUrls().Select(t => t.Address);
        //        if (_urls != null && _urls.Count() > 0)
        //        {
        //            foreach (string url in _urls)
        //            {
        //                await TalkAsync(url);
        //            }
        //        }
        //    }
        //    else if (Message.ToLower().Contains("#>Users".ToLower()))
        //    {
        //        UserManager _manager = new UserManager();
        //        var _urls = _manager.GetAll().Select(t => t.UserName);
        //        if (_urls != null && _urls.Count() > 0)
        //        {
        //            foreach (string url in _urls)
        //            {
        //                await TalkAsync(url);
        //            }
        //        }
        //    }
        //}

        
    }
}