using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoApplication.Attributes;
using DemoApplication.Interfaces;

namespace DemoApplication.Entities
{
    [TableName("SchoolTeacher")]
    public class Teacher:IEntity
    {

        #region IEntity Members
        [Identity]
        [DbColumn("ID")]
        public int ID { get; set; }
        [DbColumn("GUID")]
        public string GUID { get; set; }
        #endregion
        [DbColumn("TeacherName")]
        public string TeacherName { get; set; }
        [DbColumn("SchoolName")]
        public string SchoolName { get; set; }
        [DbColumn("Qualification")]
        public string Qualification { get; set; }
    }
}
