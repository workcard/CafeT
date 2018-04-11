using CafeT.Frameworks.Ai.VnText;
using CafeT.Html;
using CafeT.Objects;
using CafeT.Text;
using HtmlAgilityPack;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BotMessages
{
    
    public interface IBotMessage
    {
        Task<bool> ExcuteAsync();
        //Task<bool> UpdateDbAsync();
    }
    //public abstract class BotMessage
    //{
    //    public void Insert(IBotMessage message)
    //    {
            
    //    }
    //}
    public enum CardTypes
    {
        Animation,
        Audio,
        Hero,
        Thumbnail,
        Receipt,
        SignIn,
        Video
    }

    

    [Serializable]
    public class BingResultMessage : IBotMessage
    {
        public string Message { set; get; }
        public IDialogContext Context;
        public List<string> Commands { set; get; } = new List<string>();
        public List<string> Urls { set; get; } = new List<string>();

        public BingResultMessage(IDialogContext context, string message)
        {
            Context = context;
            Message = message;
            Commands = Message.ToWords().Where(t => t.StartsWith("#")).ToList();
            Urls = Message.GetUrls().ToList();
        }

        public async Task<bool> ExcuteAsync()
        {
            Activity replyToConversation = (Activity)Context.MakeMessage();
            replyToConversation.Recipient = replyToConversation.Recipient;
            replyToConversation.Type = "message";

            replyToConversation.Text = WebUtility.HtmlDecode(Message);
            replyToConversation.InputHint = "Kết quả này đúng hay sai ?";
            replyToConversation.InputHint = InputHints.ExpectingInput;
            replyToConversation.TextFormat = TextFormatTypes.Markdown;

            replyToConversation.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){ Title = "Đúng", Type=ActionTypes.ImBack, Value=true},
                    new CardAction(){ Title = "Sai", Type=ActionTypes.ImBack, Value=false }
                }
            };

            await Context.PostAsync(replyToConversation);

            try
            {
                string _vietnamese = replyToConversation.Text.ToVietnamese();
                if (_vietnamese.ToLower().ToStandard().Replace(" ", "") != replyToConversation.Text.ToLower().ToStandard().Replace(" ", ""))
                {
                    await Context.PostAsync(_vietnamese);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                string _english = replyToConversation.Text.ToEnglish();
                if (_english.ToLower().ToStandard().Replace(" ", "") != replyToConversation.Text.ToLower().ToStandard().Replace(" ", ""))
                {
                    await Context.PostAsync(_english);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
    [Serializable]
    public class TextMessage: IBotMessage
    {
        public string Message { set; get; }
        public IDialogContext Context;
        public List<string> Commands { set; get; } = new List<string>();
        public List<string> Urls { set; get; } = new List<string>();

        public TextMessage(IDialogContext context, string message)
        {
            Context = context;
            Message = message;
            Commands = Message.ToWords().Where(t => t.StartsWith("#")).ToList();
            Urls = Message.GetUrls().ToList();
        }

        public async Task<bool> ExcuteAsync()
        {
            Activity replyToConversation = (Activity)Context.MakeMessage();
            replyToConversation.Recipient = replyToConversation.Recipient;
            replyToConversation.Type = "message";
           
            replyToConversation.Text = WebUtility.HtmlDecode(Message);
            replyToConversation.InputHint = "Kết quả này đúng hay sai ?";
            replyToConversation.InputHint = InputHints.ExpectingInput;
            replyToConversation.TextFormat = TextFormatTypes.Markdown;

            replyToConversation.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){ Title = "Đúng", Type=ActionTypes.ImBack, Value=true},
                    new CardAction(){ Title = "Sai", Type=ActionTypes.ImBack, Value=false }
                }
            };
            
            await Context.PostAsync(replyToConversation);
            
            try
            {
                string _vietnamese = replyToConversation.Text.ToVietnamese();
                if(_vietnamese.ToLower().ToStandard().Replace(" ","") != replyToConversation.Text.ToLower().ToStandard().Replace(" ", ""))
                {
                    await Context.PostAsync(_vietnamese);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                string _english = replyToConversation.Text.ToEnglish();
                if (_english.ToLower().ToStandard().Replace(" ", "") != replyToConversation.Text.ToLower().ToStandard().Replace(" ", ""))
                {
                    await Context.PostAsync(_english);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }

    [Serializable]
    public class UrlMessage:IBotMessage
    {
        public string Message { set; get; }
        public List<string> Urls { set; get; } = new List<string>();
        public List<string> Commands { set; get; } = new List<string>(); //Command start with #>
        public List<string> Keywords { set; get; } = new List<string>(); //Keyword start with #

        public IDialogContext Context;

        public UrlMessage(IDialogContext context, string message)
        {
            if (message.IsContainsUrl())
            {
                Context = context;
                Message = message;
                Urls = message.GetUrls().ToList();
                Commands.Add("#>read"); //if UrlMessage is command default by #>read
                Keywords = Message.ToWords().Where(t => t.StartsWith("#")).ToList();
            }
        }

        public async Task<bool> ExcuteAsync()
        {
            string _url = Urls[0];
            WebPage _page = new WebPage(_url.Trim());
            Keywords = _page.Keywords;

            if (Commands.Count > 0)
            {
                foreach(string command in Commands)
                {
                    if(command == "#>read")
                    {
                        if (Keywords.Count > 0)
                        {
                            foreach (string keyword in Keywords)
                            {
                                if (keyword.ToLower() == "#table")
                                {
                                    if (_page.HtmlTables != null && _page.HtmlTables.Count > 0)
                                    {
                                        await Context.PostAsync("Html tables: " + _page.HtmlTables.Count.ToString());
                                        foreach(var table in _page.HtmlTables)
                                        {
                                            if(table.MaybeRate())
                                            {
                                                foreach (var _row in table.Rows)
                                                {
                                                    //await Context.PostAsync(_row);WebUtility.HtmlDecode(messageActivity.Text)
                                                    await Context.PostAsync(WebUtility.HtmlDecode(_row));
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (keyword.ToLower() == "#image")
                                {
                                    if (_page.Images != null && _page.Images.Count > 0)
                                    {
                                        await Context.PostAsync("Images : " + _page.Images.Count.ToString());
                                        await Context.PostAsync("Now, this is all images ...");
                                        if (_page.Images != null && _page.Images.Count > 0)
                                        {
                                            var _images = _page.Images;
                                            if (_images != null && _images.Count() > 0)
                                            {
                                                foreach (var _row in _images)
                                                {
                                                    Activity replyToConversation = (Activity)Context.MakeMessage();
                                                    replyToConversation.Recipient = replyToConversation.Recipient;
                                                    replyToConversation.Type = "message";

                                                    string _img = _row.GetUrls().FirstOrDefault();
                                                    if (!_img.IsNullOrEmptyOrWhiteSpace())
                                                    {
                                                        if (_img.Contains("png"))
                                                        {
                                                            _img = _img.GetFromBeginTo("png") + "png";
                                                            replyToConversation.Attachments.Add(new Attachment()
                                                            {
                                                                ContentUrl = _img,
                                                                ContentType = "image/png",
                                                                Name = "Bender_Rodriguez.png"
                                                            });
                                                            await Context.PostAsync(replyToConversation);
                                                        }
                                                        else if (_img.Contains("jpg"))
                                                        {
                                                            _img = _img.GetFromBeginTo("jpg") + "jpg";
                                                            replyToConversation.Attachments.Add(new Attachment()
                                                            {
                                                                ContentUrl = _img,
                                                                ContentType = "image/jpg",
                                                                Name = "Bender_Rodriguez.png"
                                                            });
                                                            await Context.PostAsync(replyToConversation);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                //else //Default process with #news
                                //{
                                //    await context.PostAsync(_page.HtmlContent);
                                //}
                            }
                        }
                    }
                    //Tam thoi xu ly toi day thoi
                }
            }

            return false;
        }
    }
    [Serializable]
    public class CsharpMessage : IBotMessage
    {
        public IDialogContext Context { set; get; }
        public string Message { set; get; }
        public CsharpMessage(IDialogContext context, string message)
        {
            Context = context;
            Message = message;
        }
        public object Excute(string code)
        {
            // Load code from file  
            //StreamReader sReader = new StreamReader(@"~/CodeSnips/CSharp.txt");
            //string input = sReader.ReadToEnd();
            //sReader.Close();

            // Code literal  
            //string code =
            //    @"using System;  
  
            //      namespace WinFormCodeCompile  
            //      {  
            //          public class Transform  
            //          {  
                         
            //               public string Hello(string input)  
            //               {
            //                 return "Thế giới này, là của chúng mình";
            //               }  
            //           }  
            //       }";

            // Compile code  
            CSharpCodeProvider cProv = new CSharpCodeProvider();
            CompilerParameters cParams = new CompilerParameters();
            cParams.ReferencedAssemblies.Add("mscorlib.dll");
            cParams.ReferencedAssemblies.Add("System.dll");
            cParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cParams.GenerateExecutable = false;
            cParams.GenerateInMemory = true;

            CompilerResults cResults = cProv.CompileAssemblyFromSource(cParams, code);

            // Check for errors  
            if (cResults.Errors.Count != 0)
            {
                foreach (var er in cResults.Errors)
                {
                    Console.WriteLine(er.ToString());
                }
                return "Error";
            }
            else
            {
                // Attempt to execute method.  
                object obj = cResults.CompiledAssembly.CreateInstance("WinFormCodeCompile.Transform");
                Type t = obj.GetType();
                object[] arg = { "" }; // Pass our textbox to the method  
                return t.InvokeMember("Hello", BindingFlags.InvokeMethod, null, obj, arg);
            }
        }
        public async Task<bool> ExcuteAsync()
        {
            Activity replyToConversation = (Activity)Context.MakeMessage();
            replyToConversation.Recipient = replyToConversation.Recipient;
            replyToConversation.Type = "message";

            replyToConversation.Text = Excute(Message).PrintAllProperties();
            replyToConversation.TextFormat = TextFormatTypes.Markdown;

            //Attachment cardAttachment = null;

            await Context.PostAsync(replyToConversation);
            return false;
        }
    }

    [Serializable]
    public class VideoMessage: IBotMessage
    {
        public IDialogContext Context { set; get; }
        public string Message { set; get; }
        public VideoMessage(IDialogContext context, string message)
        {
            Context = context;
            Message = message;
        }

        public async Task<bool> ExcuteAsync()
        {
            Activity replyToConversation = (Activity)Context.MakeMessage();
            replyToConversation.Recipient = replyToConversation.Recipient;
            replyToConversation.Type = "message";

            //replyToConversation.Text = WebUtility.HtmlDecode(Message);
            replyToConversation.TextFormat = TextFormatTypes.Markdown;

            Attachment cardAttachment = null;

            VideoCard videoCard = new VideoCard()
            {
                //Title = "Breaking Bad",
                //Subtitle = "Breaking Bad is an American crime drama television series created and produced by Vince Gilligan.",
                //Image = new ThumbnailUrl()
                //{
                //    Url = "https://upload.wikimedia.org/wikipedia/en/thumb/6/61/Breaking_Bad_title_card.png/250px-Breaking_Bad_title_card.png"
                //},
                Media = new List<MediaUrl>()
                            {
                                new MediaUrl()
                                {
                                    Url = $"{Message}"
                                }
                            },
                Buttons = new List<CardAction>()
                            {
                                new CardAction()
                                {
                                    Title = "Watch All Episodes",
                                    Type = ActionTypes.OpenUrl,
                                    Value = $"{Message}"
                                }
                            }
            };
            cardAttachment = videoCard.ToAttachment();
            replyToConversation.Attachments.Add(cardAttachment);

            await Context.PostAsync(replyToConversation);
            return false;
        }
    }

    [Serializable]
    public class ImageMessage
    {
        public string Title { set; get; }
        public string ImagePath { set; get; }
        public ImageMessage(string title, string path)
        { }
    }

    [Serializable]
    public class FileMessage
    {
        public string Title { set; get; }
        public string FilePath { set; get; }
        public FileMessage() { }
        public FileMessage(string title, string path)
        { }
    }

    [Serializable]
    public class HtmlMessage
    {
        public string Title { set; get; }
        public string HtmlContent { set; get; }
        public HtmlMessage() { }
        public HtmlMessage(string title, string html)
        { }
    }
}
