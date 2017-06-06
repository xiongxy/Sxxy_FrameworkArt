using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Web.ViewModels.HomeViewModels;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var vm = CreateViewModel<LoginViewModel>();
            return View();
        }
    }
}