using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using TiTiBot.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Newtonsoft.Json;
using System.Text;
using QNABot.Models;
using System.Collections.Generic;
using System.IO;
using CafeT.Frameworks.Ai.VnText;
using TiTiBot.Users;

namespace TiTiBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        //public MessagesController()
        //{
        //}
        //internal static IDialog<ContactMessage> MakeRoot()
        //{
        //    return Chain.From(() => FormDialog.FromForm(ContactMessage.BuildForm));
        //}
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            #region Set CurrentBaseURL
            // Get the base URL that this service is running at
            // This is used to show images
            string CurrentBaseURL = this.Url.Request.RequestUri.AbsoluteUri.Replace(@"api/messages", "");
            // Create an instance of BotData to store data
            BotData objBotData = new BotData();
            // Instantiate a StateClient to save BotData            
            StateClient stateClient = activity.GetStateClient();
            // Use stateClient to get current userData
            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            // Update userData by setting CurrentBaseURL and Recipient
            userData.SetProperty<string>("CurrentBaseURL", CurrentBaseURL);
            // Save changes to userData
            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            #endregion
            

            if (activity.Type == ActivityTypes.Message)
            {
                if (activity.Attachments.Count > 0)
                {
                    if (activity.Attachments[0].ContentType == "image/png")
                    {
                        activity.Text = "Image";
                        await Conversation.SendAsync(activity, () => new Dialogs.RootAMADialog());
                    }
                    else if (activity.Attachments[0].ContentType == "application/pdf")
                    {
                        activity.Text = "Pdf";
                        await Conversation.SendAsync(activity, () => new Dialogs.RootAMADialog());
                    }
                    else if (activity.Attachments[0].ContentType == "text/plain")
                    {
                        activity.Text = "txt";
                        var _file = activity.Attachments[0];

                        using(var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(_file.ContentUrl);
                            var content = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("", "login")

                            });
                            var result = await client.PostAsync("/api/Membership/exists", content);
                            string resultContent = await result.Content.ReadAsStringAsync();
                            Console.WriteLine(resultContent);
                        }

                        await Conversation.SendAsync(activity, () => new Dialogs.RootAMADialog());
                    }
                }
                else
                {
                    try
                    {

                        //await _bot.StartAsync(activity);
                        await Conversation.SendAsync(activity, () => new Dialogs.QnADialog());
                        //if (message.EndsWith("?"))
                        //{
                        //    await Conversation.SendAsync(activity, () => new Dialogs.QnADialog());
                        //}
                        //else if (message.StartsWith("#"))
                        //{
                        //    await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                        //}
                        //else
                        //{
                        //    await Conversation.SendAsync(activity, () => new Dialogs.LUISDialog());
                        //}
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            //else if (message.Type == "EndOfConversation")
            //{
            //    var hours = DateTime.Now.Hour;
            //    String partDay = (hours > 16) ? "Evening" : (hours > 11) ? "Afternoon" : "Morning";
            //    Message reply = message.CreateReplyMessage("ServBot::Good " + partDay + " User!!" +
            //        Environment.NewLine + "Local Time is: " + DateTime.Now.ToString());
            //    return reply;
            //}
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        
    }
}