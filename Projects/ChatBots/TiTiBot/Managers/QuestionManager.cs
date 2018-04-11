using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MathBot.Models;
using MathNet.Numerics.Random;

namespace MathBot.Managers
{
    [Serializable]
    public class QuestionManager
    {
        private MathBotDataContext db = new MathBotDataContext();

        public IEnumerable<Answer> GetAnswers(Guid id)
        {
            return db.Answers.Where(t => t.QuestionId == id).AsEnumerable();
        }

        public IEnumerable<Question> GetQuestions()
        {
            return db.Questions.AsEnumerable();
        }

        public bool AddQuestion(Question model)
        {
            db.Questions.Add(model);
            var _value = db.SaveChangesAsync();

            if(_value.Result >= 0)
            {
                return true;
            }
            return false;
        }

        public List<Question> GenerateRandomQuestions(int n)
        {
            List<Question> _questions = new List<Question>();
            for (int i = 0; i < n; i++)
            {
                Question _question = new Question();

                var random = new MersenneTwister(42); 
                double _a = MathNet.Numerics.Combinatorics.Combinations(i + 10, i);
                double _b = MathNet.Numerics.Combinatorics.Combinations(i + 20, i);

                _question.Content = _a.ToString() + " + " + _b.ToString();
                _questions.Add(_question);
            }
            return _questions;
        }
    }
}