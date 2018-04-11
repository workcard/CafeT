using System;
namespace DemoApplication.Attributes
{
[AttributeUsage(AttributeTargets.Property,AllowMultiple=false)]
public class DbColumnAttribute:Attribute
{
    public string Name { get; set; }
    public DbColumnAttribute(string name)
    {
        this.Name = name;
    }
}
}
