using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Models
{
    public class InterviewModel : BaseObject
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public DateTime InterviewDate { set; get; }

        public virtual IEnumerable<QuestionModel> Questions { set; get; }

        public InterviewModel() : base()
        {
        }

        public InterviewModel(string name) : base()
        {
            Name = name;
        }

        public string MakeArticleContent()
        {
            string _content = string.Empty;
            if(Questions != null && Questions.Count() > 0)
            {
                foreach (var q in Questions)
                {
                    _content += q.Title + "<br />" + q.Content;
                    if(q.Answers != null && q.Answers.Count() > 0)
                    {
                        foreach (var a in q.Answers)
                        {
                            _content += q.Title + "<hr />" + q.Content;
                        }
                    }
                    _content += "<br />";
                }
            }
            
            return _content;
        }
    }
}