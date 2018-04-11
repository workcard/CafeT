using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathBot.Dialogs
{
    [Serializable]
    public class ThumbnailCardDialog : IDialog<string>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            context.Done<object>(null);
            return Task.CompletedTask;
        }
        /// <summary>
        /// MessageReceivedAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var welcomeMessage = context.MakeMessage();
            welcomeMessage.Text = "Welcome to bot MathBot";
            await context.PostAsync(welcomeMessage);
            await DisplayThumbnailCard(context);
            context.Done<object>(null);
            return;
        }
        /// <summary>
        /// DisplayThumbnailCard
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task DisplayThumbnailCard(IDialogContext context)
        {
            var replyMessage = context.MakeMessage();
            Attachment attachment = GetProfileThumbnailCard(); ;
            replyMessage.Attachments = new List<Attachment> { attachment };
            await context.PostAsync(replyMessage);
        }
        /// <summary>
        /// GetProfileThumbnailCard
        /// </summary>
        /// <returns></returns>
        private static Attachment GetProfileThumbnailCard()
        {
            var thumbnailCard = new ThumbnailCard
            {
                Title = "MathBot",
                Subtitle = "Trợ thủ toán học",
                Tap = new CardAction(ActionTypes.OpenUrl, "Learn More", value: "http://chuyentoan.vn"),
                Text =  $"MathBot - là một phần của ChuyenToan.vn." +
                $"Tôi hỗ trợ bạn những tính toán cơ bản một cách nhanh chóng trong phần lớn khuôn khổ toán phổ thông." +
                $"Các bạn có thể xem thêm về MathBot tại http://chuyentoan.vn",
                Images = new List<CardImage>
                {
                    new CardImage("http://csharpcorner.mindcrackerinc.netdna-cdn.com/UploadFile/AuthorImage/jssuthahar20170821011237.jpg")
                },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Learn More", value: "http://chuyentoan.vn"),
                    new CardAction(ActionTypes.OpenUrl, "C# Corner", value: "http://chuyentoan.vn"),
                    new CardAction(ActionTypes.OpenUrl, "MSDN", value: "http://chuyentoan.vn")
                }
            };

            return thumbnailCard.ToAttachment();
        }
    }
}