using Mvc5.CafeT.vn.Models;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class ArticleAuthorView
    {
        public ApplicationUser UserView { set; get; }
        public ArticleAuthorView()
        {
            UserView = new ApplicationUser();
        }
    }
}