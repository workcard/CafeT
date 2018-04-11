using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CafeT.Text;
using CafeT.Objects;

namespace CafeT.BusinessObjects
{
    public abstract class BaseObject: EF6.Objects.BaseObject
    {
        [Key]
        public Guid Id { set; get; }

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

        public string Viewers { set; get; } = string.Empty;

        [ScaffoldColumn(false)]
        public DateTime? LastViewAt { set; get; }

        [ScaffoldColumn(false)]
        public string LastViewBy { set; get; }

        public BaseObject()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            CreatedBy = "Application";
            LastUpdatedBy = null;
            LastUpdatedDate = null;
            LastViewBy = null;
        }

        public virtual bool IsOf(string userName)
        {
            bool _isCreatedBy = CreatedBy.ToLower() == userName.ToLower();
            bool _isUpdatedBy = (!LastUpdatedBy.IsNullOrEmptyOrWhiteSpace()) 
                && (LastUpdatedBy.ToLower() == userName.ToLower());
            if (_isCreatedBy || _isUpdatedBy) return true;
            return false;
        }
       
        public virtual bool IsNew(int days = 3)
        {
            if (CreatedDate.AddDays(days) >= DateTime.Now)
            {
                return true;
            }
            return false;
        }

        public virtual void ToView(string userName)
        {
            if(!IsOf(userName))
            {
                CountViews = CountViews + 1;
                LastViewBy = userName;
                LastViewAt = DateTime.Now;
                if(!Viewers.IsNullOrEmptyOrWhiteSpace()
                    && !Viewers.Contains(userName))
                {
                    Viewers.AddAfter(userName + ";");
                }
                else
                {
                    Viewers.AddAfter(userName);
                }
            }
        }
        public virtual bool HasProperty(string name)
        {
            var _properties = this.GetProperties();
            if(_properties != null && _properties.Length > 0)
            {
                foreach(var _pro in _properties)
                {
                    if (_pro.Equals(name)) return true;
                }
            }
            return false;
        }
    }
}
