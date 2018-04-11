using CafeT.BusinessObjects;
using CafeT.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MathBot.Models
{
    public class Question:BaseObject
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public IEnumerable<Answer> Answers { set; get; }

        public void AddAnswer(Answer model)
        {
            Answers.ToList().Add(model);
        }

        public Question():base()
        {
        }

        public Answer CorrectAnswer { set; get; }
    }

    public class Answer:BaseObject
    {
        public string Content { set; get; }
        public Guid QuestionId { set; get; }

        public Answer() : base() { }
    }
}