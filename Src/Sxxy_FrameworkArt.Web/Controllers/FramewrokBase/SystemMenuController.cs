using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    [ControllerOrActionDescription("系统菜单管理")]
    public class SystemMenuController : BaseController
    {
        // GET: SystemMenu
        public ActionResult Index()
        {
            return View();
        }
    }
}