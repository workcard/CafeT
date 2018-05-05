using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Web.Models;

namespace Mvc5.CafeT.vn.Models
{
    public class ComplainModel:BaseObject
    {
        [MaxLength(250)]
        public string Title { set; get; }
        public string Content { set; get; }

        public Guid? AnswerId { set; get; }

        public ComplainModel() : base() { }
    }

    public class AnswerModel:BaseObject
    {
        public string Name { set; get; }
        public string Content { set; get; }
        public bool IsCorrect { set; get; }

        public decimal? Mark { set; get; }
        public Guid? QuestionId { set; get; }

        
        public virtual bool IsOfQuestion(Guid id)
        {
            if (this.QuestionId.HasValue && this.QuestionId.Value == id)
            {
                return true;
            }
            return false;
        }

        public virtual IEnumerable<ComplainModel> Complains { set; get; }
        public virtual IEnumerable<FileModel> Files { set; get; }
        public virtual IEnumerable<AnswerReviewModel> Reviews { set; get; }

        public AnswerModel():base()
        {
            IsCorrect = false;
        }

        public bool HasComplains()
        {
            if (Complains != null && Complains.Count() > 0) return true;
            return false;
        }

        public bool HasReviews()
        {
            if (Reviews != null && Reviews.Count() > 0) return true;
            return false;
        }

        public bool HasFiles()
        {
            if (Files != null && Files.Count() > 0) return true;
            return false;
        }

        public bool HasMark()
        {
            if (Mark.HasValue) return true;
            return false;
        }

        public void CalcMark()
        {
            Mark = (decimal)Reviews.Where(t => t.Marks.HasValue)
                .Select(t => t.Marks).Average();
        }

        public void ToUpdate()
        {
            CalcMark();
            UpdatedDate = DateTime.Now;
        }

        //public void ToView()
        //{
        //    CountViews = CountViews + 1;
        //    LastViewAt = DateTime.Now;
        //}
    }
}