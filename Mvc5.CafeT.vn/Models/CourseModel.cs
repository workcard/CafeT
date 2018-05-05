
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Mvc5.CafeT.vn.Models
{
    public class CourseModel:BaseObject
    {
        public virtual ImageModel Image { set; get; }
        public IEnumerable<ArticleModel> Articles { set; get; }
        public IEnumerable<ImageModel> Images { set; get; }
        public IEnumerable<QuestionModel> Questions { set; get; }
        public IEnumerable<FileModel> Files { set; get; }

        public string Tags { set; get; }

        public CourseModel():base()
        {
            IsEnable = false;
        }

        public CourseModel(string title):base()
        {
            Name = title;
            IsEnable = false;
        }

        public string Name { set; get; }
        public string Description { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { set; get; }

        public bool IsEnable { set; get; }
    }
}