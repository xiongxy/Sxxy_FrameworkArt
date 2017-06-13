using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages
{
    public static class BootstrapExtension
    {

        public static MvcHtmlString RouteGuidance<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, string modeulName)
        {
            Bootstrap.BootstrapRouteGuidance obj = new Bootstrap.BootstrapRouteGuidance()
            {
                ModulName = modeulName
            };
            var rv = html.InnerHelper.Editor("", "BootstrapRouteGuidance", new { obj = obj });
            return rv;
        }


        public static MvcForm BeginForm<TViewModel>(this BootstrapHtmlHelper<TViewModel> html)
        {
            StringBuilder sb = new StringBuilder();

            MvcForm mf = new MvcForm(html.InnerHelper.ViewContext);
            return mf;
        }
    }
}
