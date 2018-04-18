using CafeT.BusinessObjects.ELearning;
using System.Collections.Generic;

namespace Mvc5.CafeT.vn.Models
{
    public class CourseModel:Course
    {
        public virtual ImageModel Image { set; get; }
        public IEnumerable<ArticleModel> Articles { set; get; }
        public IEnumerable<ImageModel> Images { set; get; }
        public IEnumerable<QuestionModel> Questions { set; get; }
        public IEnumerable<FileModel> Files { set; get; }

        public string Tags { set; get; }

        public CourseModel()
        {
            IsEnable = false;
        }

        public CourseModel(string title) : base(title)
        {
            IsEnable = false;
        }
    }
}