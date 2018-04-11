using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.ComponentModel;
using Google.Apis.Translate.v2;
using TranslationsResource = Google.Apis.Translate.v2.Data.TranslationsResource;
using System.Collections.Generic;
using Google.Apis.Services;
using CafeT.Text;
using System.Linq;
using CafeT.Objects;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using TiTiBot.Messages;
using CafeT.GoogleServices;
using AutoMapper;
using System.Text;
using System.Net.Sockets;
using CafeT.Html;
using Microsoft.Bot.Builder.FormFlow;
using System.Threading;
using TiTiBot.Services;
using CafeT.BotMessages;

namespace TiTiBot.Dialogs
{
    [Serializable]
    public class BaseDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }
        public async Task BotTalk(IDialogContext context, string msg)
        {
            IBotMessage message;
            if (msg.IsContainsUrl())
            {
                message = new UrlMessage(msg);
            }
            else
            {
                TextMessage _message = new TextMessage();
                _message.Title = string.Empty;
                _message.Content = msg;
                await context.PostAsync(msg);
                await context.PostAsync("Finished. Pls continue chat with me");
            }
        }
    }
}