using System;
using DemoApplication.Attributes;
using DemoApplication.Interfaces;

namespace DemoApplication.Entities
{
    [TableName("StudentDetails")]
    public class Student : IEntity
    {
        #region IEntity Members
        [Identity]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("GUID")]
        public string GUID { get; set; }
        #endregion

        [DbColumn("Name")]
        public string Name { get; set; }

        [DbColumn("Age")]
        public string Age { get; set; }

        [DbColumn("Email")]
        public string Email { get; set; }

    }
}
