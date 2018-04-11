using BestMatchDialog;
using CafeT.Objects;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TiTiBot.Services;

namespace TiTiBot.Dialogs
{
    [LuisModel("6438e43093424af2b60b84ac2e15f702", "6438e43093424af2b60b84ac2e15f702")]
    [Serializable]
    public class CommonResponsesDialog : BestMatchDialog<object>
    {
        public string DefaultMessage = "";
        public string IntroMessage = "";

        [BestMatch(new string[] { "Hi", "Hi There", "Hello there", "Hey", "Hello",
            "Hey there", "Greetings", "Good morning", "Good afternoon", "Good evening", "Good day" },
            threshold: 0.5, ignoreCase: false, ignoreNonAlphaNumericCharacters: false)]
        public async Task HandleGreeting(IDialogContext context, string messageText)
        {
            await context.PostAsync("Well hello there. What can I do for you today?");
            context.Wait(MessageReceived);
        }

        [BestMatch(new string[] { "how goes it", "how do", "hows it going", "how are you",
            "how do you feel", "whats up", "sup", "hows things" })]
        public async Task HandleStatusRequest(IDialogContext context, string messageText)
        {
            await context.PostAsync("I am great.");
            context.Wait(MessageReceived);
        }

        [BestMatch(new string[] { "bye", "bye bye", "got to go", "see you later", "laters", "adios" })]
        public async Task HandleGoodbye(IDialogContext context, string messageText)
        {
            await context.PostAsync("Bye. Looking forward to our next awesome conversation already.");
            context.Wait(MessageReceived);
        }

        [BestMatch("thank you|thanks|much appreciated|thanks very much|thanking you", listDelimiter: '|')]
        public async Task HandleThanks(IDialogContext context, string messageText)
        {
            await context.PostAsync("You're welcome.");
            context.Wait(MessageReceived);
        }

        [BestMatch("#stock", listDelimiter: '|')]
        public async Task HandleStockCommand(IDialogContext context, string messageText)
        {
            //string StockRateString = await FinanceService.GetStock(messageText.Replace("#Stock ", ""));
            string StockRateString = await FinanceService.GetStock("ibm");
            await context.PostAsync(StockRateString);
            context.Wait(MessageReceived);
        }

        [BestMatch("#where", listDelimiter: '|')]
        public async Task HandleWhereCommand(IDialogContext context, string messageText)
        {
            await context.PostAsync(context.PrintAllProperties());
            context.Wait(MessageReceived);
        }

        public override async Task NoMatchHandler(IDialogContext context, string messageText)
        {
            await context.PostAsync($"I’m not sure what you want when you say '{messageText}'.");
            context.Call(new RootDialog(), this.AfterHelloCompleted);
        }
        private async Task AfterHelloCompleted(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
    }
}