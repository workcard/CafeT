
using System.Collections.Generic;
using Web.Models;

namespace Mvc5.CafeT.vn.Models
{
    public class ProjectModel:BaseObject
    {
        //public ImageModel Image { set; get; }
        public List<ArticleModel> Articles { set; get; }
        public List<QuestionModel> Questions { set; get; }
        public List<FileModel> Files { set; get; }
        public string Tags { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        public ProjectModel()
        {
            Files = new List<FileModel>();
            Articles = new List<ArticleModel>();
            Questions = new List<QuestionModel>();

        }

        public ProjectModel(string title)
        {
            Name = title;
            Files = new List<FileModel>();
            Articles = new List<ArticleModel>();
            Questions = new List<QuestionModel>();
        }
    }
}