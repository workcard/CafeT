using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace TiTiBot.Dialogs
{
    [LuisModel("f30e2ba8-5d6e-471f-b934-6aec62dc604e", "6438e43093424af2b60b84ac2e15f702")]
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
        public string Message { set; get; } = string.Empty;
        public override async Task StartAsync(IDialogContext context)
        {
            string message = $"Now, i'm using LUIS ";
            await context.PostAsync(message);
            context.Wait<Activity>(MessageReceived);
        }
        private async Task ResumeAfterLuisDialog(IDialogContext context, IAwaitable<object> result)
        {
            // Set BaseURL
            //context.UserData.TryGetValue<string>("CurrentBaseURL", out strBaseURL);

            var activity = await result as Activity;
            string message = activity.Text;
            string lowerMessage = activity.Text.ToLower();

            //context.Done<object>(null); -- Nếu muốn kết thúc thằng quăng nó ở đây
            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Finished. Pls continue chat with me");
            context.Wait(MessageReceivedAsync);
        }

        
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand " + result.Query;
            await context.PostAsync(message);
        }

        [LuisIntent("number")]
        public async Task Numbers(IDialogContext context, LuisResult result)
        {
            if (result.Entities.Count < 1)
                await HandleUnknownIntent(context, $"Bạn đã nhập vào không phải là số " + result.Query);

            foreach(var entity in result.Entities)
            {
                await context.PostAsync(entity.Resolution.FirstOrDefault().Value.ToString());
            }
        }


        [LuisIntent("Addition")]
        public async Task Addition(IDialogContext context, LuisResult result)
        {
            if (result.Entities.Count != 2)
                await HandleUnknownIntent(context, $"Sorry I did not understand " + result.Query);

            await context.PostAsync((float.Parse(result.Entities[0].Resolution.Values.First().ToString()) + float.Parse(result.Entities[1].Resolution.Values.First().ToString())).ToString());
        }

        [LuisIntent("Subtraction")]
        public async Task Subtraction(IDialogContext context, LuisResult result)
        {
            if (result.Entities.Count != 2)
                await HandleUnknownIntent(context, $"Sorry I did not understand " + result.Query);

            await context.PostAsync((float.Parse(result.Entities[0].Resolution.Values.First().ToString()) - float.Parse(result.Entities[1].Resolution.Values.First().ToString())).ToString());
        }

        [LuisIntent("Multiplication")]
        public async Task Multiplication(IDialogContext context, LuisResult result)
        {
            if (result.Entities.Count != 2)
                await HandleUnknownIntent(context, $"Sorry I did not understand " + result.Query);

            await context.PostAsync((float.Parse(result.Entities[0].Resolution.Values.First().ToString()) * float.Parse(result.Entities[1].Resolution.Values.First().ToString())).ToString());
        }

        [LuisIntent("Division")]
        public async Task Division(IDialogContext context, LuisResult result)
        {
            if (result.Entities.Count != 2)
                await HandleUnknownIntent(context, $"Sorry I did not understand " + result.Query);

            await context.PostAsync((float.Parse(result.Entities[0].Resolution.Values.First().ToString()) / float.Parse(result.Entities[1].Resolution.Values.First().ToString())).ToString());
        }

        public async Task HandleUnknownIntent(IDialogContext context, string message)
        {
            await context.PostAsync(message);
            context.Wait<Activity>(MessageReceived);
        }
    }
}