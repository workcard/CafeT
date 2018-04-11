using AutoMapper;
using GenericObjects;
using GenericObjects.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BusinessObjects.Crawler
{
    public class Url:BaseObject
    {
        public string BaseUrl { set; get; }
        public string UrlLink { set; get; }
        public string Name { set; get; }
        public string SelectWords { set; get; }
        public string RejectWords { set; get; }

        public string UrlCss { set; get; }
        public string TitleCss { set; get; }
        public string DescriptionCss { set; get; }
        public string ContentCss { set; get; }
        public string TagsCss { set; get; }
        public string PostTimeCss { set; get; }
        public string PostByCss { set; get; }
        public string UpdateTimeCss { set; get; }
        public string UpdateByCss { set; get; }

        public string MetaCss { set; get; }
        public string CategoryCss { set; get; }

        public bool Enable { set; get; }
        /// <summary>
        /// The css class for selected items in webpage - url
        /// </summary>
        public string CssClassItems { set; get; }
        //public Guid? CategoryId { set; get; }

        public string MarkEndTrim { set; get; } //The start of word to end of content when fecth from url
        public string MarkStartTrim { set; get; } //The start of word to end of content when fecth from url
        public string Nodes { set; get; }

        public Url()
        {
            this.Id = Guid.NewGuid();
            this.BaseUrl = string.Empty;
            this.CreatedBy = string.Empty;
            this.CreatedDate = DateTime.Now;
            this.LastUpdatedBy = string.Empty;
            this.LastUpdated = DateTime.Now;
            this.Name = string.Empty;
            this.CssClassItems = string.Empty;

            this.TitleCss = string.Empty;
            this.DescriptionCss = string.Empty;
            this.ContentCss = string.Empty;
            this.TagsCss = string.Empty;

            this.PostTimeCss = string.Empty;
            this.PostByCss = string.Empty;
            this.UpdateTimeCss = string.Empty;
            this.UpdateByCss = string.Empty;

            this.MetaCss = string.Empty;
            this.CategoryCss = string.Empty;

            //Default value to split with {";"}
            this.SelectWords = ";"; 
            this.RejectWords = ";";

            this.MarkStartTrim = string.Empty;
            this.MarkEndTrim = string.Empty;

            this.Enable = false;

            this.Nodes = string.Empty;
        }

        public GenericUrl ToGeneric()
        {
            GenericUrl _view = new GenericUrl();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Url, GenericUrl>());
            var mapper = config.CreateMapper();
            _view = mapper.Map<GenericUrl>(this);
            return _view;
        }

        public List<string> InternalLinks()
        {
            List<string> _links = new List<string>();
            return _links;
        }
        public List<string> ExternalLinks()
        {
            List<string> _links = new List<string>();
            return _links;
        }
    }
}