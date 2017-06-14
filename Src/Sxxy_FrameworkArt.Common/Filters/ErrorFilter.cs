using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Sxxy_FrameworkArt.Common.Helpers;

namespace Sxxy_FrameworkArt.Common.Filters
{
    public class ErrorFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                filterContext.ExceptionHandled = true;
                var routeData = new RouteData();
                routeData.Values["prefix"] = "WebApi";
                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = "Show";
                routeData.Values["exception"] = filterContext.Exception;
                IController errorsController = Utils.GetErrorController();
                var rc = new RequestContext(filterContext.HttpContext, routeData);
                errorsController.Execute(rc);
            }
        }
    }
}
