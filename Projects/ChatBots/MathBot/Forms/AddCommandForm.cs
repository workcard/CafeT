using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using CafeT.Text;

namespace MathBot
{
    [Serializable]
    public class AddCommandForm
    {
        // these are the fields that will hold the data
        // we will gather with the form
        [Prompt("Name? {||}")]
        public string Name;
        [Prompt("Description? {||}")]
        public string Description;
     
        // This method 'builds' the form 
        // This method will be called by code we will place
        // in the MakeRootDialog method of the MessagesControlller.cs file
        public static IForm<AddCommandForm> BuildForm()
        {
            return new FormBuilder<AddCommandForm>()
                    .Message("Bạn đang thêm Contact mới. Hãy điền các thông tin sau: ")
                    .OnCompletion(async (context, form) =>
                    {
                        context.PrivateConversationData.SetValue<string>(
                            "Name", form.Name);
                        context.PrivateConversationData.SetValue<string>(
                            "Description", form.Description);
                        // Tell the user that the form is complete
                        await context.PostAsync("Ok. Bạn đã hoàn thành");
                    })
                    .Build();
        }
    }
}