using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TiTiBot.Messages
{
    public enum BotCommand
    {
        Start,
        UpdateGroup,
        Reset,
        End,
        ResetGroup,
        Update
    }
    public interface IBotCommandHanlder
    {
        BotCommand Command { get; }
        Task Handle(IDialogContext bot, string message);
    }
    public abstract class BaseCommandHandler : IBotCommandHanlder
    {
        public BotCommand Command { get; private set; }
        public BaseCommandHandler(BotCommand command)
        {
            Command = command;
        }
        public abstract Task Handle(IDialogContext bot, string message);
    }
    public class StartBotCommandHandler : BaseCommandHandler
    {
        //private readonly DataService _dataService;
        public StartBotCommandHandler(BotCommand command) : base(command)
        {
        }
        public override async Task Handle(IDialogContext bot, string message)
        {
            //await __dataService.AddChatId(bot.Activity);
            await bot.PostAsync("Ok");
        }
        //https://ait.codes/2017/06/bot-me-gai-phan-2/
        //public async Task AddChatId(IActivity activity)
        //{
        //    var chatIds = await GetChatIds();
        //    if (chatIds.Any(e => e.ConversationId == activity.Conversation.Id))
        //        return;
        //    _context.Chats.Add(new Chat()
        //    {
        //        Subcriv
        //    }
        //}
    }
    public class BotCommandHandlerList:List<IBotCommandHanlder>
    {
        public async Task Process(IDialogContext bot, string chanel, string message)
        {
            //var messageText = Helpers.GetMessage(message);
            string messageText = message;
            string command = messageText.Split(' ')[0]
                .ToLower()
                .Trim();
            var handler = this.FirstOrDefault(e => e.Command.ToString().ToLower() == "command");
            if(handler != null)
            {
                try
                {
                    await handler.Handle(bot, messageText);
                }
                catch(Exception ex)
                {
                    await bot.PostAsync(ex.StackTrace);
                }
            }
        }
    }
    public class BotMessage
    {
        public readonly static string DefaultResponseMessage =
            $"Hi, I'm TiTiBot! I support the following commands: {Environment.NewLine}" +
            $"- '#Search' - base on Google Search. {Environment.NewLine}" +
            $"- '#Translate or #Trans - base on Google translate. {Environment.NewLine}" +
            $"- 'who's next' - Indicates who has the next selection. {Environment.NewLine}" +
            $"- 'recommendation' - Get a random recommendation in the area. {Environment.NewLine}";

        public readonly static string SayGoodbyeResponseMessage =
            $"Thanks you for using TiTiBot: {Environment.NewLine}" +
            $"I'm hope meet you later {Environment.NewLine}";
    }
}