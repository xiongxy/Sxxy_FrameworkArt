using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Web.ViewModels.HomeViewModels;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    [ActionDescription(DisplayName = "登录")]
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
           
            return View();
        }
    }
}