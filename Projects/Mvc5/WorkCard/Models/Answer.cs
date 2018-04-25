using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Answer : BaseObject
    {
        [DataType(DataType.MultilineText)]
        public string Content { set; get; }

        public Guid QuestionId { set; get; }
        public Answer() : base() { }
    }
}