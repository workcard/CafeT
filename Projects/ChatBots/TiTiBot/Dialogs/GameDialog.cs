using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using CafeT.Objects;
using System.Threading;
using CafeT.Html;
using CafeT.Text;

namespace TiTiBot.Dialogs
{
    [Serializable]
    public class GameDialog : IDialog<object>
    {
        public string DialogName = "GameDialog";

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(this.PrintAllProperties());
            context.Wait(MessageReceivedAsync);
        }
        //private async Task AfterHelloCompleted(IDialogContext context, IAwaitable<object> result)
        //{
        //    context.Done<object>(null);
        //}
        private async Task AfterNumberGuesserDialogCompleted(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
            //context.Call(new RootDialog(), this.AfterHelloCompleted);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            // Set BaseURL
            //context.UserData.TryGetValue<string>("CurrentBaseURL", out strBaseURL);
            var activity = await result as Activity;
            string message = activity.Text;
            string lowerMessage = activity.Text.ToLower();
            
            //if (lowerMessage == "hi")
            //{
            //    await context.PostAsync("Welcome");
            //}
            if (lowerMessage.StartsWith("#HotGirl".ToLower()))
            {
                List<string> _imageWebs = new List<string>();
                _imageWebs.Add("http://www.xemanh.net/phi-huyen-trang-thanh-nghien-my-go");
                foreach (string _web in _imageWebs)
                {
                    WebPage _page = new WebPage(_web);
                    if (_page != null && _page.Images != null && _page.Images.Count > 0)
                    {
                        var _jpgImages = _page.Images.Where(t => t.Contains("jpg"));
                        if (_jpgImages != null && _jpgImages.Count() > 0)
                        {
                            if (_jpgImages.Count() > 12)
                            {
                                _jpgImages = _jpgImages.Skip(2).Take(10);
                            }
                            foreach (var _image in _jpgImages)
                            {
                                await context.PostAsync(_image.GetUrls()[0]);
                                //await BotTalk(context, _image.GetUrls()[0]);
                            }
                        }
                    }
                }
            }
            else
            {
                await context.Forward(new NumberGuesserDialog(), this.AfterNumberGuesserDialogCompleted, message, CancellationToken.None);
            }
        }
    }
}