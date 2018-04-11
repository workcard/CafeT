using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace TiTiBot.Models
{
    public abstract class BaseObject
    {
        [Key]
        public string Id { set; get; }

        [DisplayName("Created on")]
        [ScaffoldColumn(false)]

        public DateTime CreatedDate { set; get; }

        [ScaffoldColumn(false)]
        public string CreatedBy { set; get; }

        [ScaffoldColumn(false)]
        public DateTime? LastUpdatedDate { set; get; }

        [ScaffoldColumn(false)]
        public string LastUpdatedBy { set; get; }

        [ScaffoldColumn(false)]
        public int CountViews { set; get; } = 0;

        public IEnumerable<string> Viewers { set; get; }

        [ScaffoldColumn(false)]
        public DateTime? LastViewAt { set; get; }

        [ScaffoldColumn(false)]
        public string LastViewBy { set; get; }

        public BaseObject()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            CreatedBy = "Application";
            LastUpdatedBy = null;
            LastUpdatedDate = null;
            LastViewBy = null;
        }
    }
    //public class BaseObject
    //{
    //    [Key]
    //    public Guid Id { set; get; } 
        
    //    public string PathTextDb { set; get; }
    //    public DateTime CreatedDate { set; get; }
    //    public string CreatedBy { set; get; }
    //    public DateTime? LastUpdatedDate { set; get; }

        
    //    public BaseObject()
    //    {
    //        Id = Guid.NewGuid();
    //        CreatedDate = DateTime.Now;
    //        CreatedBy = "Application";
    //        PathTextDb = string.Empty;
    //    }

    //    public TimerObject ToTimerObject()
    //    {
    //        return new TimerObject(this);
    //    }
    //    public string GetObjectInfo()
    //    {
    //        return this.GetObjectAllFields();
    //    }
    //    public void Print()
    //    {
    //        this.PrintAllProperties();
    //    }
    //}
}
