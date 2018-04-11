using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text;
using System.Collections.Generic;
using CafeT.Text;
using AutoMapper;
using System.Linq;
using TiTiBot.Models;

namespace TiTiBot.Dialogs
{
    [Serializable]
    public class ProfileDialog : IDialog<object>
    {
        string strBaseURL;
        protected int intNumberToGuess;
        protected int intAttempts;

        #region public async Task StartAsync(IDialogContext context)
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("This is ProfileDialog");
            context.Wait(MessageReceivedAsync);
        }
        #endregion

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            // Set BaseURL
            context.UserData.TryGetValue<string>(
                "CurrentBaseURL", out strBaseURL);

            // Get the text passed
            var activity = await result as IMessageActivity;
            string message = activity.Text;

            if (message.ToLower().StartsWith("#Update Email".ToLower()))
            {
                string _email = activity.Text.GetEmails().FirstOrDefault();
                using (Models.TiTiBotDataContext dataContext = new Models.TiTiBotDataContext())
                {
                    var newActivity = Mapper.Map<IMessageActivity, Models.ActivityBo>(activity);
                    var _user = dataContext.Users.Where(t => t.UserName == newActivity.FromName).FirstOrDefault();
                    if (_user != null)
                    {
                        _user.Email = _email;
                        try
                        {
                            dataContext.SaveChanges();
                            await context.PostAsync(_user.UserName + " | " + _user.Email);
                            await context.PostAsync("Đã cập nhật thông tin");
                            var _users = dataContext.Users;
                            foreach (var _item in _users)
                            {
                                await context.PostAsync(_item.UserName + " | " + _item.Email);
                            }
                        }
                        catch (Exception ex)
                        {
                            await context.PostAsync(ex.Message);
                        }
                    }
                }
            }
        }
        private async Task AfterHelloCompleted(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }

        
    }
}