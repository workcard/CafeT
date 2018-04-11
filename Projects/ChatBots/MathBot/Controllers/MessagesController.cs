using CafeT.Objects;
using CafeT.Text;
using MathBot.Dialogs;
using MathBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace MathBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity != null)
            {
                using (MathBotDataContext dbContext = new MathBotDataContext())
                {
                    ActivityModel model = new ActivityModel();
                    model.Activity = activity.ToJson();
                    dbContext.Activities.Add(model);
                    await dbContext.SaveChangesAsync();
                }

                string message = activity.Text;
                string _fullText = activity.Text;
                var _tokens = _fullText.GetHasTags();
                var _emails = _fullText.GetEmails();

                if (activity.Type == ActivityTypes.Message)
                {
                    StateClient sc = activity.GetStateClient();
                    BotData userData = sc.BotState.GetPrivateConversationData(activity.ChannelId,
                        activity.Conversation.Id, activity.From.Id);
                    await Conversation.SendAsync(activity, () => new RootDialog());
                }
                else
                {
                    HandleSystemMessage(activity);
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private Activity HandleSystemMessage(Activity activity)
        {
            var client = new ConnectorClient(new Uri(activity.ServiceUrl),
                                                        new MicrosoftAppCredentials());
            var reply = activity.CreateReply();

            switch (activity.GetActivityType())
            {
                case ActivityTypes.ConversationUpdate:
                    IConversationUpdateActivity update = activity;
                    if (update.MembersAdded != null && update.MembersAdded.Any())
                    {
                        foreach (var newMember in update.MembersAdded)
                        {
                            if (newMember.Id != activity.Recipient.Id)
                            {
                                reply.Text = $"Welcome {newMember.Name}!";
                                client.Conversations.ReplyToActivityAsync(reply);
                            }
                        }
                    }
                    break;
                case ActivityTypes.ContactRelationUpdate:
                    IContactRelationUpdateActivity relationUpdate = activity;
                    reply.Text = "RelationUpdate";
                    client.Conversations.ReplyToActivityAsync(reply);
                    break;
                case ActivityTypes.Typing:
                    ITypingActivity typing = activity;
                    reply.Text = "Typing";
                    client.Conversations.ReplyToActivityAsync(reply);
                    break; 
                case ActivityTypes.DeleteUserData:
                case ActivityTypes.Ping:
                    break;
                default:
                    Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");
                    break;
            }
            return null;
        }
    }
}
