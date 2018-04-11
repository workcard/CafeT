using System;
namespace DemoApplication.Attributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=false)]
    public class IdentityAttribute:Attribute
    {
        public bool IsID { get; set; }
        public IdentityAttribute()
        {
            this.IsID = true;
        }
    }
}
