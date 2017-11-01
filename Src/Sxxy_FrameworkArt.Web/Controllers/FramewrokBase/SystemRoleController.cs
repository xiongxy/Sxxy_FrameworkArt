using System;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Web.ViewModels.System.SystemRoleViewModels;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    [ControllerOrActionDescription("系统用户管理")]
    public class SystemRoleController : BaseController
    {
        [ControllerOrActionDescription("查询用户")]
        public ActionResult Index()
        {
            var vm = CreateViewModel<SystemRoleListViewModel>();
            return PartialView(vm);
        }
    }
}