using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text;
using System.Collections.Generic;

namespace TiTiBot.Dialogs
{
    [Serializable]
    public class NumberGuesserDialog : IDialog<object>
    {
        string strBaseURL;
        protected int intNumberToGuess;
        protected int intAttempts;

        #region public async Task StartAsync(IDialogContext context)
        public async Task StartAsync(IDialogContext context)
        {
            // Generate a random number
            Random random = new Random();
            this.intNumberToGuess = random.Next(1, 6);

            // Set Attempts
            this.intAttempts = 1;

            // Start the Game
            context.Wait(MessageReceivedAsync);
        }
        #endregion

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            // Set BaseURL
            context.UserData.TryGetValue<string>(
                "CurrentBaseURL", out strBaseURL);

            int intGuessedNumber;

            // Get the text passed
            var message = await argument;

            // See if a number was passed
            if (!int.TryParse(message.Text, out intGuessedNumber))
            {
                // A number was not passed  

                // Create a reply Activity
                Activity replyToConversation = (Activity)context.MakeMessage();
                replyToConversation.Recipient = replyToConversation.Recipient;
                replyToConversation.Type = "message";

                string strNumberGuesserCard =
                    String.Format(@"{0}/{1}",
                    strBaseURL,
                    "Images/NumberGuesserCard.png");

                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage(url: strNumberGuesserCard));

                // Create the Buttons
                // Call the CreateButtons utility method
                List<CardAction> cardButtons = CreateButtons();

                // Create the Hero Card
                // Set the image and the buttons
                HeroCard plCard = new HeroCard()
                {
                    Images = cardImages,
                    Buttons = cardButtons,
                };

                // Create an Attachment by calling the
                // ToAttachment() method of the Hero Card                
                Attachment plAttachment = plCard.ToAttachment();
                // Attach the Attachment to the reply
                replyToConversation.Attachments.Add(plAttachment);
                // set the AttachmentLayout as 'list'
                replyToConversation.AttachmentLayout = "list";

                // Send the reply
                await context.PostAsync(replyToConversation);
                context.Wait(MessageReceivedAsync);
            }

            // This code will run when the user has entered a number
            if (int.TryParse(message.Text, out intGuessedNumber))
            {
                // A number was passed
                // See if it was the correct number
                if (intGuessedNumber != this.intNumberToGuess)
                {
                    // The number was not correct
                    this.intAttempts++;

                    // Create a response
                    // This time call the ** ShowButtons ** method
                    Activity replyToConversation =
                        ShowButtons(context, "Not correct. Guess again.");

                    await context.PostAsync(replyToConversation);
                    context.Wait(MessageReceivedAsync);
                }
                else
                {
                    // Game completed
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Congratulations! ");
                    sb.Append("The number to guess was {0}. ");
                    sb.Append("You needed {1} attempts. ");
                    sb.Append("Would you like to play again?");

                    string CongratulationsStringPrompt =
                        string.Format(sb.ToString(),
                        this.intNumberToGuess,
                        this.intAttempts);

                    // Put PromptDialog here
                    PromptDialog.Confirm(
                        context,
                        PlayAgainAsync,
                        CongratulationsStringPrompt,
                        "Didn't get that!");
                }
            }
        }
        private async Task AfterHelloCompleted(IDialogContext context, IAwaitable<object> result)
        {
            //if ((await result) == true)
            //{
            //    //Exit the root dialog gracefully
            //    context.Done<object>(null);
            //}
            context.Done<object>(null);
        }
        private async Task PlayAgainAsync(IDialogContext context, IAwaitable<bool> result)
        {
            // Generate new random number
            Random random = new Random();
            this.intNumberToGuess = random.Next(1, 6);

            // Reset attempts
            this.intAttempts = 1;

            // Get the response from the user
            var confirm = await result;

            if (confirm) // They said yes
            {
                // Start a new Game
                // Create a response
                // This time call the ** ShowButtons ** method
                Activity replyToConversation = ShowButtons(context, "Hi Welcome! - Guess a number between 1 and 5");
                await context.PostAsync(replyToConversation);
                context.Wait(MessageReceivedAsync);
            }
            else // They said no
            {
                await context.PostAsync("Goodbye!");
                context.Call(new RootDialog(), this.AfterHelloCompleted);
                //context.Wait(MessageReceivedAsync);
            }
        }

        // Utility

        #region private static List<CardAction> CreateButtons()
        private static List<CardAction> CreateButtons()
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
        private static Activity ShowButtons(IDialogContext context, string strText)
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