using CafeT.BusinessObjects;
using CafeT.Text;
using MathBot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace MathBot.Models
{
    public class DynamicBot:BaseObject
    {
        public string Name { set; get; }
        public string About { set; get; }
        public string SourceCode { set; get; }
        public string SourceFile { set; get; }
        public bool IsRunning { set; get; } = false;
        public List<string> Errors { set; get; }

        public void Build()
        {
        }
        public void Start()
        {
            IsRunning = true;
        }
        public void Stop()
        {
            IsRunning = false;
        }
        public DynamicBot(string code) : base()
        {

        }
        private void Execute(string code)
        {
            StringBuilder sb = new StringBuilder();
            //-----------------
            // Create the class as usual
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine("{");

            sb.AppendLine("      public class SelectCountries");
            sb.AppendLine("      {");
            sb.AppendLine("            public string GetHello(string message)");
            sb.AppendLine("            {");
            sb.AppendLine("            return \"Hello\";");
            //sb.AppendLine(code);
            sb.AppendLine("            }");
            sb.AppendLine("      }");
            sb.AppendLine("}");

            //-----------------
            // The finished code
            String classCode = sb.ToString();

            //-----------------
            // Dont need any extra assemblies
            Object[] requiredAssemblies = new Object[] { };

            dynamic classRef;
            try
            {
                Errors.Clear();

                //------------
                // Pass the class code, the namespace of the class and the list of extra assemblies needed
                classRef = CodeHelper.HelperFunction(classCode, "CountryList.SelectCountries", requiredAssemblies);

                //-------------------
                // If the compilation process returned an error, then show to the user all errors
                if (classRef is CompilerErrorCollection)
                {
                    StringBuilder sberror = new StringBuilder();

                    foreach (CompilerError error in (CompilerErrorCollection)classRef)
                    {
                        sberror.AppendLine(string.Format("{0}:{1} {2} {3}",
                                           error.Line, error.Column, error.ErrorNumber, error.ErrorText));
                    }

                    Errors.Add(sberror.ToString());
                    return;
                }
            }
            catch (Exception ex)
            {
                // If something very bad happened then throw it
                Errors.Add(ex.Message);
                throw;
            }
        }
    }

    public class BotModel:BaseObject
    {
        public string Name { set; get; } = string.Empty;
        public string About { set; get; } = string.Empty;
        public IDialogContext Context { set; get; }

        public string BotResponse { set; get; } = string.Empty;
        public string UserRequest { set; get; } = string.Empty;

        public BotModel(IDialogContext context, string userRequest) : base()
        {
            Context = context;
            UserRequest = userRequest;
            BuildBotMessage();
        }

        public void RecognizeUserRequest()
        {

        }

        private void BuildBotMessage()
        {
            BotResponse = string.Empty;
        }

        public void SayHello()
        {
            BotResponse = "Hello";
        }
        public void SayGoodbye()
        {
            BotResponse = "Goodbye";
        }
        public void SendEmail(string toEmail)
        {
        }
        public async Task BotTalkAsync()
        {
            if(!BotResponse.IsNullOrEmptyOrWhiteSpace())
            {
                await Context.PostAsync($"{BotResponse}");
                return;
            }
        }
    }


    //public class Conversation : BaseObject
    //{
    //    public ConversationStarter Starter { set; get; }
    //    public Dictionary<string, List<string>> Messages { set; get; }
    //    public UserMessage userMessage { set; get; }
    //    public BotMessage botMessage { set; get; }
    //    public Conversation() : base()
    //    {
    //        Starter = new ConversationStarter();
    //        Messages = new Dictionary<string, List<string>>();
    //    }
    //}
}