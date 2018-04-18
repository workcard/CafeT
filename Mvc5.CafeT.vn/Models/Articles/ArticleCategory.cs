using CafeT.BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Models
{
    public class ArticleCategory:BaseObject
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public bool? IsDisplay { set; get; }
        public IEnumerable<ArticleModel> Articles { set; get; }

        public ArticleCategory():base()
        {
            IsDisplay = false;
        }

        public bool HasArticles()
        {
            if (Articles != null && Articles.Count() > 0) return true;
            return false;
        }
    }
}