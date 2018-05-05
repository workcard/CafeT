using CafeT.Enumerable;
using CafeT.Time;
using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Helpers
{
    public static class ArticleHelper
    {
        public static IEnumerable<ArticleModel> GetAllAfter(this IEnumerable<ArticleModel> collection, DateTime time)
        {
            return collection.Where(t => t.CreatedDate.Value.IsLaterOrEquals(time)).OrderByDescending(t=>t.CreatedDate);
        }
        public static IEnumerable<ArticleModel> SearchBy(this IEnumerable<ArticleModel> collection, string searchString)
        {
            return collection.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Summary != null && s.Summary.ToLower().Contains(searchString.ToLower()))
                            || (s.Tags != null && s.Tags.ToLower().Contains(searchString.ToLower())))
                            .OrderByDescending(t => t.CreatedDate).ToList();

        }
        public static IEnumerable<ArticleModel> GetTopViews(this IEnumerable<ArticleModel> collection, int? n)
        {
            return collection.OrderByDescending(c=>c.CountViews).TakeMax(n);
        }
        public static IEnumerable<ArticleModel> GetTopNews(this IEnumerable<ArticleModel> collection, int? n)
        {
            return collection.OrderByDescending(c => c.CreatedDate).TakeMax(n);
        }
        public static IEnumerable<ArticleModel> GetAllBefore(this IEnumerable<ArticleModel> collection, DateTime time)
        {
            return collection.Where(t => !t.CreatedDate.Value.IsLaterOrEquals(time)).OrderByDescending(t => t.CreatedDate);
        }
    }
}