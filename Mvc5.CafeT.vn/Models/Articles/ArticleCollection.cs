using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class ArticleCollection : List<ArticleModel>
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
    }
}