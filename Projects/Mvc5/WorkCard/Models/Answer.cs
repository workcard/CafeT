using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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