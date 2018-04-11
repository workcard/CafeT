using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using CafeT.Text;

namespace MathBot
{
    [Serializable]
    public class AddContactForm
    {
        // these are the fields that will hold the data
        // we will gather with the form
        [Prompt("What is your first name? {||}")]
        public string FirstName;
        [Prompt("What is your last name? {||}")]
        public string LastName;
        [Prompt("Email ? {||}")]
        public string Email;

        // This method 'builds' the form 
        // This method will be called by code we will place
        // in the MakeRootDialog method of the MessagesControlller.cs file
        public static IForm<AddContactForm> BuildForm()
        {
            return new FormBuilder<AddContactForm>()
                    .Message("Bạn đang thêm Contact mới. Hãy điền các thông tin sau: ")
                    .OnCompletion(async (context, form) =>
                    {
                        // Set BotUserData
                        context.PrivateConversationData.SetValue<bool>(
                            "ProfileComplete", true);
                        context.PrivateConversationData.SetValue<string>(
                            "FirstName", form.FirstName);
                        context.PrivateConversationData.SetValue<string>(
                            "LastName", form.LastName);

                        if(form.Email.IsEmail())
                        {
                            context.PrivateConversationData.SetValue<string>(
                            "Email", form.Email);
                        }
                        // Tell the user that the form is complete
                        await context.PostAsync("Ok. Bạn đã hoàn thành");
                    })
                    .Build();
        }
    }
}