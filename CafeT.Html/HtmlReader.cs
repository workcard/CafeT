using CafeT.SmartObjects;
using CafeT.Text;
using HtmlAgilityPack;
using System.Linq;
using Vereyon.Web;

namespace CafeT.Html
{
    public class HtmlProcessor
    {
        public string Input { set; get; } = string.Empty;
        public string CleanHtml { set; get; } = string.Empty;
        public string PlanText { set; get; } = string.Empty;
        public HtmlReader Reader { set; get; }

        public HtmlProcessor(string htmlString)
        {
            Input = htmlString;
            Reader = new HtmlReader(Input);
            CleanHtml = Reader.CleanHtml;
            MakePlanText();
        }

        public void MakePlanText()
        {
            HtmlDocument document = new HtmlDocument();
            document.OptionFixNestedTags = true;
            document.LoadHtml(CleanHtml);
            HtmlToText convert = new HtmlToText();
            PlanText = convert.Convert(document.DocumentNode.InnerHtml);
        }

        public string MakeHtml(string planText)
        {
            return planText.ToHtml();
        }
    }

    public class HtmlReader
    {
        public string HtmlInput { set; get; } = string.Empty;
        public string CleanHtml { set; get; } = string.Empty;
        HtmlDocument document;
        public HtmlReader(string htmlString)
        {
            HtmlInput = htmlString;
            document = new HtmlDocument();
            document.OptionFixNestedTags = true;
            document.LoadHtml(HtmlInput);
            ToHtmlStandard();
        }

        public bool IsHtmlStandard()
        {
            if (document.ParseErrors.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public void ToHtmlStandard()
        {
            if(!IsHtmlStandard())
            {
                TextProcessor processor = new TextProcessor(document.DocumentNode.InnerText);
                HtmlToText convert = new HtmlToText();
                string content = convert.Convert(document.DocumentNode.InnerHtml);
                CleanHtml = content.ToHtml();
            }
        }
    }
}
