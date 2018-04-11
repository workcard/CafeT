using CafeT.Html;
using CafeT.Text;
using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class Story : BaseObject, ICloneable
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string Content { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }
        public Guid? ProjectId { set; get; }
        public Story() : base() { }
        
        public Story PrepareToView()
        {
            Story _story = (Story)this.Clone();
            if (_story.Description.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Content.IsNullOrEmptyOrWhiteSpace())
                {
                    _story.Description = Content.TakeMaxWords(100);
                }
            }
            else
            {
                _story.Description = Description.TakeMaxWords(100);
            }
            return _story;
        }

        public object Clone()
        {
            Story _story = (Story)this.MemberwiseClone();
            return _story;
        }
    }
}