using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sxxy_FrameworkArt.Common.Filters
{
    /// <summary>
    /// 权限过滤器
    /// </summary>
    public class PrivilegeFilter : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// 在访问Action之前执行的动作
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.GetType().IsSubclassOf(typeof(BaseController)) == false)
                return;
            base.OnActionExecuting(filterContext);
        }
    }
}
