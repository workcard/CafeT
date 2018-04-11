using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using CafeT.Text;

namespace MathBot
{
    [Serializable]
    public class AddRequestForm
    {
        public string Message;

        // This method 'builds' the form 
        // This method will be called by code we will place
        // in the MakeRootDialog method of the MessagesControlller.cs file
        public static IForm<AddRequestForm> BuildForm()
        {
            return new FormBuilder<AddRequestForm>()
                    .Message("Cảm ơn bạn đã gửi yêu cầu đến ChuyenToan.vn. Chúng tôi sẽ nghiên cứu và cập nhật sớm nhất có thể.")
                    //.
                    .OnCompletion(async (context, form) =>
                    {
                        await context.PostAsync("Cảm ơn bạn");
                    })
                    .Build();
        }
    }
}