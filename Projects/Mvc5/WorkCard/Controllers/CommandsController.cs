using CafeT.Html;
using CafeT.Text;
using Repository.Pattern.UnitOfWork;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class CommandsController : BaseController
    {
        public CommandsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        [HttpGet]
        [Authorize]
        public ActionResult ExcuteCommand(string command)
        {
            string _command = command.GetFromTo("{", "}");
            //string st1 = HttpUtility.HtmlEncode(_command);
            _command = HttpUtility.HtmlDecode(_command);

            string _result = string.Empty;
            if (_command.IsEmail())
            {
                _result = ContactManager.GetByEmail(_command).Email;
            }
            else
            {
                _result = ContactManager.SearchByName(_command).FirstOrDefault().Email;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CommandResult", _result);
            }
            return View("_CommandResult", _result);
        }
    }
}