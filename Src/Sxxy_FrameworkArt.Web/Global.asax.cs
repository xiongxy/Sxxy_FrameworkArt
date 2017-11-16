using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sxxy_FrameworkArt.Common;

namespace Sxxy_FrameworkArt.Web
{
    public class MvcApplication : BaseApplication
    {
        public MvcApplication()
        {
            ControllerNamsSpace = "Sxxy_FrameworkArt.Web.Controllers";
        }
    }
}
