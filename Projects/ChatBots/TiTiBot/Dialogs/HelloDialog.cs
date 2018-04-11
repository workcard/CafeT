using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Connector;
using System.Text;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace TiTiBot.Dialogs
{
    [LuisModel("6438e43093424af2b60b84ac2e15f702", "6438e43093424af2b60b84ac2e15f702")]
    [Serializable]
    public class HelloDialog : IDialog<object>
    {
        private int counter = 0;

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceivedAsync);
        }


        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message.Text.ToLower() == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Are you sure you want to reset the counter?",
                    "Didn't get that!");
            }
            else
            {
                var length = (message.Text ?? "").Length;
                await context.PostAsync($"[{++counter}] Hello! You said \"{message.Text}\" ({length} characters)");
                context.Call(new RootDialog(), this.None);
            }
        }
        private async Task None(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
        public async Task AfterResetAsync(IDialogContext context,IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                counter = 0;
                await context.PostAsync("Reset counter.");
            }
            else
            {
                await context.PostAsync("Did not reset counter.");
            }
            context.Wait(MessageReceivedAsync);
        }
    }

}