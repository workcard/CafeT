
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Web.Models;

namespace Mvc5.CafeT.vn.Models
{
    public enum QuestionType
    {
        YesNo,
        Writting
    }

    public class QuestionModel:BaseObject
    {
        [MaxLength(250)]
        public string Title { set; get; }

        public string Content { set; get; }

        public string Authors { set; get; }
        public bool IsVerified { set; get; }
        public string VerifiedBy { set; get; }
        public DateTime? VerifiedDate { set; get; }

        public string Tags { set; get; }

       
        public virtual bool CanAddAnswer()
        {
            if (IsVerified)
            {
                return true;
            }
            return false;
        }
        public bool IsNews(int days)
        {
            if (this.CreatedDate.Value.AddDays(days) >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
        public int? Level { set; get; }

        public int? Marks { set; get; }

        public Guid? ArticleId { set; get; }
        public Guid? ExamId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? ProductId { set; get; }
        public Guid? JobId { set; get; }
        public Guid? InterviewId { set; get; }

        /// <summary>
        /// Minutes
        /// </summary>
        public int? EstimationTime { set; get; }

        public QuestionType Type { set; get; }
        public virtual IEnumerable<FileModel> Files { set; get; }
        public virtual IEnumerable<AnswerModel> Answers { set; get; }
        public virtual AnswerModel CorrectAnswer { set; get; }

        public QuestionModel():base()
        {
            Title = string.Empty;
            Content = string.Empty;
            IsVerified = false;
            EstimationTime = 45;
            Level = 0;
            Marks = 0;
        }

        public bool IsOfArticle(Guid id)
        {
            if (this.ArticleId.HasValue && this.ArticleId.Value == id)
                return true;
            return false;
        }
        public bool IsOfInterview(Guid id)
        {
            if (this.InterviewId.HasValue && this.InterviewId.Value == id)
                return true;
            return false;
        }

        public bool IsNoAnswer()
        {
            if (Answers == null) return true;
            return false;
        }

        public bool HasFiles()
        {
            if (Files != null && Files.Count() > 0) return true;
            return false;
        }

        public bool HasAnswer()
        {
            if (Answers != null && Answers.Count() > 0) return true;
            return false;
        }

        //public override string ToString()
        //{
        //    return this.PrintAllProperties();
        //}
    }
}