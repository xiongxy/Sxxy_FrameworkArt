using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Web.ViewModels.System.SystemUserViewModels;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    [ControllerOrActionDescription("系统用户管理", "")]
    public class SystemUserController : BaseController
    {
        // GET: SystemUser
        public ActionResult Index()
        {
            SystemUserListViewModel bb = new SystemUserListViewModel();
            var vm = CreateViewModel<SystemUserListViewModel>();
            return View(vm);
        }
    }
}
