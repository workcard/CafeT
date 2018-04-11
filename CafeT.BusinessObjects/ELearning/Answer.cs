using System;
using System.Collections.Generic;

namespace CafeT.BusinessObjects.ELearning
{
    public class Answer : BaseObject
    {
        public string Name { set; get; }
        public string Content { set; get; }
        public bool IsCorrect { set; get; }

        public decimal? Mark {set;get;}
        public Guid? QuestionId { set; get; }

        public Answer():base()
        {
            IsCorrect = false;
        }

        public virtual bool IsOfQuestion(Guid id)
        {
            if(this.QuestionId.HasValue && this.QuestionId.Value == id)
            {
                return true;
            }
            return false;
        }
    }
}
