namespace MathBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Connector;
    using MathBot.Models;
    using MathBot.Queries;
    using CafeT.Mathematics;
    using MathBot.Managers;
    using System.Threading;
    using CafeT.Text;

    [Serializable]
    public class MathDialog : IDialog<object>
    {
        [NonSerialized]
        private QuestionManager _manager = new QuestionManager();
        [NonSerialized]
        private List<Question> Questions;

        private int i = 0;
        private int n = 0;
        public bool IsContinue = true;
        public int NumbersOfQuestions = 15;
        protected List<string> UserStories { set; get; }
        protected List<string> BotStories { set; get; }
        MathEngine mathEngine = new MathEngine();
        public MathDialog()
        {
            _manager = new QuestionManager();
            Questions = _manager.GenerateRandomQuestions(NumbersOfQuestions);
            n = Questions.Count();
            UserStories = new List<string>();
            BotStories = new List<string>();
        }

        public async Task BotTalk(IDialogContext context, string msg)
        {
            await context.PostAsync(msg);
            BotStories.Add(msg);
        }

        public async Task BotAnswer(IDialogContext context, string question)
        {
            string _answer = "Bạn đã hỏi. Tôi đang tìm câu trả lời";
            if (question.EndsWith("?")) question = question.Trim().Remove(question.Length - 1, 1).Trim();
            if (question.EndsWith("=")) question = question.Trim().Remove(question.Length - 1, 1).Trim();
            MathEngine mathEngine = new MathEngine();
            if (mathEngine.IsMathExpression(question))
            {
                var _expr = mathEngine.Calc(question);
                _answer = _expr;
                await BotTalk(context, _answer);
                BotStories.Add(_answer);
            }
            else
            {
                string _msg = "Xin lỗi. Biểu thức toán học của bạn không đúng. Vui lòng hỏi lại.";
                await BotTalk(context, _msg);
                BotStories.Add(_msg);
            }

            string _functionName = question.GetFromBeginTo("(");
            //string _functionParams = question.GetB
        }

        public Question Next(int i)
        {
            try
            {
                return Questions[i];
            }
            catch
            {
                return null;
            }
        }
        
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            _manager = new QuestionManager();
            Questions = new List<Question>();
            Questions=  _manager.GenerateRandomQuestions(NumbersOfQuestions);

            if (message.Text.EndsWith("?"))
            {
                await BotAnswer(context, message.Text);
            }
            else if (message.Text.IsNumeric())
            {
                var _lastQuestion = Questions[i].Content;
                var _expr = mathEngine.Calc(_lastQuestion);
                var _answer = _expr;

                if (_answer.ToLower() == message.Text.ToLower())
                {
                    await context.PostAsync($"Đúng");
                }
                else
                {
                    await context.PostAsync($"Sai");
                }
                i = i + 1;
                await BotAsk(context, i);
            }
            else if (message.Text.Contains("test"))
            {
                await BotAsk(context, i);
            }
            else
            {
                await new RootDialog().MessageReceivedAsync(context, result);
            }
        }
        private async Task ResumeAfterMathsDialog(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            // Store the value that NewOrderDialog returned. 
            // (At this point, new order dialog has finished and returned some value to use within the root dialog.)
            var resultFromNewOrder = await result;

            await context.PostAsync($"New order dialog just told me this: {resultFromNewOrder}");

            // Again, wait for the next message from the user.
            context.Wait(this.MessageReceivedAsync);
        }
        private async Task BotAsk(IDialogContext context, int t)
        {
            if (t < n)
            {
                var question = Questions.ElementAt(t);
                var resultMessage = context.MakeMessage();

                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();
                HeroCard heroCard = CreateCard(question);
                resultMessage.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(resultMessage);
            }
            else
            {
                IsContinue = false;
            }
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Xin chào. Tôi đang sẵn sàng đợi lệnh Tính toán của bạn.!");
            IsContinue = true;
            Questions = _manager.GenerateRandomQuestions(NumbersOfQuestions);
            n = Questions.Count();
            context.Wait(this.MessageReceivedAsync);
            //var hotelsFormDialog = FormDialog.FromForm(this.BuildMathsForm, FormOptions.PromptInStart);
            //context.Call(hotelsFormDialog, this.ResumeAfterMathsFormDialog);
        }

        public async Task ContinueAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            i = i + 1;
            var _lastQuestion = Questions.ElementAt(i).Content;
            var _expr = mathEngine.Calc(_lastQuestion);
            var _result = _expr;


            if (_result.ToLower() == message.Text.ToLower())
            {
                await context.PostAsync($"Đúng");
            }
            else
            {
                await context.PostAsync($"Sai");
            }
            
            await context.PostAsync($"Câu hỏi {i} tiếp theo: ");

            if (i < n)
            {
                var question = Questions.ElementAt(i);
                var resultMessage = context.MakeMessage();

                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();
                HeroCard heroCard = CreateCard(question);
                resultMessage.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(resultMessage);   
            }
            else
            {
                IsContinue = false;
            }
        }
       

        private IForm<MathQuery> BuildMathsForm()
        {
            OnCompletionAsyncDelegate<MathQuery> processHotelsSearch = async (context, state) =>
            {
                string _text = state.Text;
                var _expr = mathEngine.Calc(_text);
                var _result = _expr;
                await context.PostAsync($"Kết quả là: {_result} ");
            };
            IsContinue = true;
            return new FormBuilder<MathQuery>()
                .Field(nameof(MathQuery.Text))
                .Message("Bạn muốn tính toán biểu thức nào {Text}...")
                .AddRemainingFields()
                .OnCompletion(processHotelsSearch)
                .Build();
        }

        //protected async Task ResumeAfterMathsFormDialog(IDialogContext context, IAwaitable<MathQuery> result)
        //{
        //    var message = await result;
        //    if(!IsContinue)
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        string _text = message.Text;
        //        var _expr = MathEngine.ToMathExpr(_text);
        //        if(_expr != null)
        //        {
        //            var _result = MathEngine.GetResult(_expr);
        //            await context.PostAsync($"Kết quả là: {_result} ");
        //        }
                
        //        await context.PostAsync($"Câu hỏi {i} tiếp theo: ");
        //        if(i<n)
        //        {
        //            _manager = new QuestionManager();
        //            Questions = _manager.GenerateRandomQuestions(NumbersOfQuestions);
        //            n = Questions.Count();

        //            var question = Questions.ElementAt(i);
        //            var resultMessage = context.MakeMessage();

        //            resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        //            resultMessage.Attachments = new List<Attachment>();
        //            HeroCard heroCard = CreateCard(question);
        //            resultMessage.Attachments.Add(heroCard.ToAttachment());

        //            await context.PostAsync(resultMessage);
        //            i = i+1;
        //        }
        //        else
        //        {
        //            IsContinue = false;
        //        }
        //    }
        //    catch (FormCanceledException ex)
        //    {
        //        string reply;

        //        if (ex.InnerException == null)
        //        {
        //            reply = "You have canceled the operation. Quitting from the HotelsDialog";
        //        }
        //        else
        //        {
        //            reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
        //        }
        //        await context.PostAsync(reply);
        //    }
        //    finally
        //    {
        //        context.Done<object>(null);
        //    }

        //}

        private static HeroCard CreateCard(Question question)
        {
            HeroCard heroCard = new HeroCard();
            heroCard.Title = "Tính giá trị biểu thức";
            heroCard.Text = question.Content;

            var _action = new CardAction()
            {
                Title = "More details",
                Type = ActionTypes.OpenUrl,
                Value = 2
                //Value = $"https://www.bing.com/search?q=hotels+in+" + HttpUtility.UrlEncode(question.Title)
            };
            heroCard.Buttons.Add(_action);
            return heroCard;
        }
    }
}