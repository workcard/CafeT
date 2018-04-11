using GenericObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Crawler
{
    public class CssUrl : BaseObject
    {
        public string Name { set; get; }
        public string CssConfig { set; get; }
        public string UrlCss { set; get; }
        public string TitleCss { set; get; }
        public string DescriptionCss { set; get; }
        public string ContentCss { set; get; }
        public string TagsCss { set; get; }
        public string PostTime { set; get; }
        public string UpdateTime { set; get; }
        public string Meta { set; get; }
        public string Category { set; get; }

        public void LoadCssConfig()
        {
            string[] _lines = File.ReadAllLines(this.CssConfig);
            foreach (string line in _lines)
            {
                if (line.Contains("|"))
                {
                    string _key = line.Split(new char[] { '|' })[0];
                    string _value = line.Split(new char[] { '|' })[1];
                    if (_key == "Url")
                    {
                        this.UrlCss = _value;
                    }
                    if (_key == "Title")
                    {
                        this.TitleCss = _value;
                    }
                    if (_key == "Description")
                    {
                        this.DescriptionCss = _value;
                    }
                    if (_key == "Content")
                    {
                        this.ContentCss = _value;
                    }
                    if (_key == "Tags")
                    {
                        this.TagsCss = _value;
                    }
                    if (_key == "PostDate")
                    {
                        this.PostTime = _value;
                    }
                    if (_key == "Category")
                    {
                        this.Category = _value;
                    }
                    if (_key == "Meta")
                    {
                        this.Meta = _value;
                    }
                    if (_key == "UpdateTime")
                    {
                        this.UpdateTime = _value;
                    }
                }
            }
        }

    }
}
