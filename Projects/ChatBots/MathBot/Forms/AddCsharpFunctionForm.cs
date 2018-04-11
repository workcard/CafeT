using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using CafeT.Text;
using MathBot.Managers;
using MathBot.Models;
using System.ComponentModel.DataAnnotations;

namespace MathBot
{
    [Serializable]
    public class AddCsharpFunctionForm
    {
        // these are the fields that will hold the data
        // we will gather with the form
        //[Prompt("What is function name ? {||}")]
        public string None; //To solve about "auto pass the first field"

        [Required]
        public string Name;
        //[Prompt("What is about this function ? {||}")]
        [Required]
        public string Description;
        [Required]
        //[Prompt("Full Code ? {||}")]
        public string FullBody;

        

        // This method 'builds' the form 
        // This method will be called by code we will place
        // in the MakeRootDialog method of the MessagesControlller.cs file
        public static IForm<AddCsharpFunctionForm> BuildForm()
        {
            
            OnCompletionAsyncDelegate<AddCsharpFunctionForm> Save = async (context, state) =>
            {
                CodeManager _manager = new CodeManager();
                CodeFunction _user = new CodeFunction();

                _user.Name = context.PrivateConversationData.GetValue<string>("Name");
                _user.Description = context.PrivateConversationData.GetValue<string>("Description");
                _user.FullBody = context.PrivateConversationData.GetValue<string>("FullBody");

                await _manager.AddFunctionAsync(_user);

                await context.PostAsync("Đã thêm được rồi bạn nhé.");
            };

            return new FormBuilder<AddCsharpFunctionForm>()
                    .Message("Bạn đang thêm Contact mới. Hãy điền các thông tin sau: ")
                    .Field(nameof(None), "")
                    .Field(nameof(Name), "Tên của hàm ?")
                    .Field(nameof(Description), "Hàm này làm gì ?")
                    .Field(nameof(FullBody), "Nội dung đầy đủ (bao gồm cả namespace) ?")
                    .OnCompletion(async (context, form) =>
                    {
                        await Save(context, form);
                    })
                    .Build();
        }
    }
}