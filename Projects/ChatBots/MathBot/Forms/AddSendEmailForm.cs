using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using CafeT.Text;
using CafeT.Emails;
using CafeT.Objects;
using Microsoft.Bot.Connector;

namespace MathBot
{
    [Serializable]
    public class AddSendEmailForm
    {
        public string Message;
        public string Email;

        // This method 'builds' the form 
        // This method will be called by code we will place
        // in the MakeRootDialog method of the MessagesControlller.cs file
        public static IForm<AddSendEmailForm> BuildForm()
        {

            OnCompletionAsyncDelegate<AddSendEmailForm> SendEmail = async (context, state) =>
            {
                var _text = context.GetEmailsFromObject();
                new OutlookEmailService().SendAsMarkDown("Chào ku nè");
                await context.PostAsync("Đã gửi email rồi bạn nhé.");
            };

            return new FormBuilder<AddSendEmailForm>()
                    .Message("Bạn muốn gửi email ?")
                     .OnCompletion(async (context, form) =>
                     {
                         if (form.Email.IsEmail())
                         {
                             context.PrivateConversationData.SetValue<string>(
                             "Email", form.Email);
                         }
                         // Tell the user that the form is complete
                         await context.PostAsync("Ok. Bạn đã hoàn thành");
                     })
                    .OnCompletion(SendEmail)
                    .Build();
        }
    }
}