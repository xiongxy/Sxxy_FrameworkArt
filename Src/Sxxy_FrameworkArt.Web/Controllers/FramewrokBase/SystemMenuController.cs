using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    public class SystemMenuController : BaseController
    {
        // GET: SystemMenu
        public ActionResult Index()
        {
            return View();
        }
    }
}