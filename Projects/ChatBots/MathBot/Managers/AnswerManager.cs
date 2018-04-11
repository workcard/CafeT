using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MathBot.Models;

namespace MathBot.Managers
{
    public class AnswerManager
    {
        private MathBotDataContext db = new MathBotDataContext();

        public IEnumerable<Answer> GetAnswers(Guid id)
        {
            return db.Answers.Where(t => t.QuestionId == id).AsEnumerable();
        }

        public bool AddAnswer(Answer model)
        {
            db.Answers.Add(model);
            var _value = db.SaveChangesAsync();

            if(_value.Result >= 0)
            {
                return true;
            }
            return false;
        }
    }
}