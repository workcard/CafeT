using CafeT.Text;
using CafeT.Time;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public class JobApplier:BaseObject
    {
        public string UserName { set; get; }
        public Guid? JobId { set; get; }
        public JobApplier():base()
        { }
    }

    public class Job:BaseObject//, ICloneable
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Content { set; get; }
        public Money Salary { set; get; } = new Money() {  Amout = 0, Currency = CurrencyType.VND };
        public Address Location { set; get; }

        public DateTime? Start { set; get; }
        public DateTime? End { set; get; }
        
        public JobStatus Status { set; get; }

        public virtual IEnumerable<Comment> Comments { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }
        public virtual List<JobApplier> Appliers { set; get; }

        public string[] Tags { set; get; }
       

        public Job():base()
        {
            Status = JobStatus.New;
            Appliers = new List<JobApplier>();
        }

        public void AddApplier(string userName)
        {
            if(!Appliers.Select(t=>t.UserName).Contains(userName))
            {
                Appliers.Add(new JobApplier() { UserName = userName, CreatedBy=this.CreatedBy });
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

        //public Job ToView()
        //{
        //    Job _model = (Job)this.Clone();
        //    return _model;
        //}

        //public object Clone()
        //{
        //    Job _story = (Job)this.MemberwiseClone();
        //    return _story;
        //}
    }
}