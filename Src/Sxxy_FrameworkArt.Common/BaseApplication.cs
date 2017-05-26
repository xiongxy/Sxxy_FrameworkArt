using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common.Filters;

namespace Sxxy_FrameworkArt.Common
{
    public class BaseApplication : System.Web.HttpApplication
    {
        public virtual void RegisterGlobalFilters(GlobalFilterCollection filter)
        {
            //填加异常过滤器
            filter.Add(new ErrorFilter());
            //检查webconfig，如果EnableLog设定为true，则添加日志过滤器
            var ls = System.Configuration.ConfigurationManager.AppSettings["EnableLog"];
            if (ls != null && (ls.ToLower() == "true" || ls == "1"))
            {
              filter.Add(new ActionLogFilter());
            }
            //添加权限过滤器
            filter.Add(new PrivilegeFilter());
        }
    }
}
