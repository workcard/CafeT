
using DemoApplication.Attributes;
using DemoApplication.Interfaces;
namespace DemoApplication.Entities
{
[TableName("ContactDetails")]
public class Contact:IEntity
{
    #region IEntity Members
    [Identity]
    [DbColumn("ID")]
    public int ID { get; set; }
    [DbColumn("GUID")]
    public string GUID { get; set; }
    #endregion
    [DbColumn("PersonName")]
    public string Name { get; set; }
    [DbColumn("Address")]
    public string Address { get; set; }
    [DbColumn("EmailId")]
    public string Email { get; set; }
}
}
