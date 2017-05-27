using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Sxxy_FrameworkArt.Common.Binders;
using Sxxy_FrameworkArt.Common.Filters;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Common
{
    public class BaseApplication : System.Web.HttpApplication
    {
        public static string ControllerNamsSpace = "";
        /// <summary>
        /// 注册过滤器
        /// </summary>
        /// <param name="filter"></param>
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
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="route"></param>
        public virtual void RegisterRoutes(RouteCollection route)
        {
            //忽略
            route.IgnoreRoute("");
            //将默认首页设置为/home/Index
            route.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { ControllerNamsSpace + ".*" }
            );
        }
        /// <summary>
        /// 注册绑定器
        /// </summary>
        public virtual void RegisterBinders()
        {
            ModelBinders.Binders.Add(typeof(string), new StringBinder());
        }
        /// <summary>
        /// 注册数据权限
        /// </summary>
        public virtual void RegisterDataPrivilege()
        {
            BaseController.DataPrivileges = new List<SupportClasses.IDataPrivilege>();
        }
        /// <summary>
        /// 开始程序
        /// </summary>
        protected void Application_Start()
        {
            var constructorInfo = BaseController.DefaultDataContext.GetConstructor(Type.EmptyTypes);
            var dataContext = (IDataContext)constructorInfo.Invoke(null);
            //获取所有不需要权限就可以访问的链接，并且保存到BaseController的全局变量中。
            BaseController.AllRightActions = Utils.GetAllAccessUrls();
            //注册默认区域
            AreaRegistration.RegisterAllAreas();
            //注册过滤器
            RegisterGlobalFilters(GlobalFilters.Filters);
            //注册路由 
            RegisterRoutes(RouteTable.Routes);
            //注册绑定器
            RegisterBinders();
            //注册数据权限
            RegisterDataPrivilege();
            //尝试访问以下数据库，以便在系统启动时候先进行对数据库的检查，从而提高启动后的效率
            try
            {
                dataContext.Set<SystemUser>().Find(1);
            }
            catch (Exception e)
            {
                //记日志, 后期使用log4net
            }
        }
    }
}
