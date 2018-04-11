namespace MathBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using CafeT.Html;
    using CafeT.Text;

    [Serializable]
    public class UrlDialog : IDialog<object>
    {
        public const string HELLO = "Bạn muốn làm gì ?";
        public string input = string.Empty;
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(HELLO);
            context.Wait(this.MessageReceivedAsync);
            
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (message.Text.IsUrl())
            {
                input = message.Text;
                string[] _links = input.LoadHtmlAsync().Result.GetUrls();
                if(_links != null && _links.Length > 0)
                {
                    foreach(string _link in _links)
                    {
                        if(_link.StartsWith("http://")|| _link.StartsWith("https://"))
                        {
                            await context.PostAsync(_link);
                        }
                    }
                }
            }
        }
    }
}
