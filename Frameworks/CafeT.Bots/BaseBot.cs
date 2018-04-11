using CafeT.BotMessages;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Bots
{
    [Serializable]
    public abstract class BaseBot
    {
       
        public IDialogContext Context;
        public List<IBotMessage> BotStories = new List<IBotMessage>();
        public BaseBot() { }
        public BaseBot(IDialogContext context)
        {
            Context = context;
        }
        public async Task TalkAsync(string message)
        {
            await Context.PostAsync($"{message}");
        }
        public void Talk(IBotMessage message)
        {
            BotStories.Add(message);
            message.ExcuteAsync();
        }
        #region private static List<CardAction> CreateButtons()
        public List<CardAction> CreateButtons()
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
        public Activity ShowButtons(IDialogContext context, string strText)
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
    }
}
