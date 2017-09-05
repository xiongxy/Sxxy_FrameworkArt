using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Web.ViewModels.System.SystemMenuViewModels;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    [ControllerOrActionDescription("系统菜单管理")]
    public class SystemMenuController : BaseController
    {
        // GET: SystemMenu
        public ActionResult Index()
        {
            var vm = CreateViewModel<SystemMenuListViewModel>();
            return PartialView(vm);
        }
    }
}