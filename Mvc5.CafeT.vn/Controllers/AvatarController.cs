//using EyeOpen.Imaging;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class AvatarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string imageBase)
        {
            //byte[] byteArray = imageBytes;
            //var file = new FileContentResult(byteArray, "image/jpeg");

            return View();
        }
    }
}