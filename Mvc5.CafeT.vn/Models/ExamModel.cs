using CafeT.BusinessObjects.ELearning;
using System.Collections.Generic;

namespace Mvc5.CafeT.vn.Models
{
    public class ExamModel:Exam
    {
        public IEnumerable<QuestionModel> Questions { set; get; }
        public ExamModel():base()
        {
        }
    }
}