using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using CafeT.Objects;

namespace TiTiBot.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog<object>
    {
        public string DialogName = "WelcomeDialog";

        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync(this.PrintAllProperties());
            await context.PostAsync("This is Welcome Dialog");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            // Set BaseURL
            //context.UserData.TryGetValue<string>("CurrentBaseURL", out strBaseURL);

            var activity = await result as IMessageActivity;
            string message = activity.Text;
            string lowerMessage = activity.Text.ToLower();
            await context.PostAsync("You say: " + message);
        }
    }
}