using CafeT.Text;
using CafeT.Time;
using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class Money
    {
        public int Amout { set; get; }
        public CurrencyType Currency { set; get; } = CurrencyType.VND;
    }

    public enum CurrencyType
    {
        VND,
        USD
    }
    public enum JobStatus
    {
        New,
        Expired,
        Open
    }

    public class Job:BaseObject, ICloneable
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Content { set; get; }
        public string Message { set; get; } = string.Empty;
        public string Owner { set; get; }
        public Money Salary { set; get; } = new Money() {  Amout = 0, Currency = CurrencyType.VND };
        public double Price { set; get; } = 0;
        public Address Location { set; get; }

        public DateTime? Start { set; get; } = DateTime.Now;
        public DateTime? End { set; get; }
        
        public JobStatus Status { set; get; } = JobStatus.New;

        public virtual IEnumerable<Comment> Comments { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }
        public List<string> Appliers { set; get; }

        public string[] Tags { set; get; }
       

        public Job():base()
        {
            Appliers = new List<string>();
        }

        public bool IsOf(string userName)
        {
            if ((!this.CreatedBy.IsNullOrEmptyOrWhiteSpace() && (this.CreatedBy.ToLower() == userName))
                || (!this.Owner.IsNullOrEmptyOrWhiteSpace() && (this.Owner.ToLower() == userName))) return true;
            return false;
        }

        public void AddApplier(string userName)
        {
            if(!Appliers.Contains(userName))
            {
                Appliers.Add(userName);
            }
        }
        
        public bool IsStandard()
        {
            bool _isStandard = this.Start.HasValue && this.End.HasValue;
            return _isStandard;
        }

        public bool IsExpired()
        {
            if (End.HasValue && End.Value.IsExpired()) return true;
            return false;
        }

        public Job ToView()
        {
            Job _model = (Job)this.Clone();
            //_model.Salary.ToReadable();
            return _model;
        }

        public object Clone()
        {
            Job _story = (Job)this.MemberwiseClone();
            return _story;
        }
    }
}