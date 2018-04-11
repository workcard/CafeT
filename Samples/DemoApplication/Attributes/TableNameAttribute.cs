using System;
namespace DemoApplication.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
    public class TableNameAttribute : Attribute
    {
        public string Name { get; set; }
        public TableNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
