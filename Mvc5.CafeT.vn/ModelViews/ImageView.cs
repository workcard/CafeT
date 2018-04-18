using Mvc5.CafeT.vn.Models;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class ImageView
    {
        public ImageModel Model { set; get; }

        public ImageView(ImageModel model)
        {
            Model = model;
        }
    }
}