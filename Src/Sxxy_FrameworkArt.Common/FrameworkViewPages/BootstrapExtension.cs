using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages
{
    public static class BootstrapExtension
    {
        /// <summary>
        /// RouteGuidance 路径导航
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html">BootstrapHtmlHelper</param>
        /// <param name="modulName">当前模块名称</param>
        /// <param name="modulRemark">当前模块说明</param>
        /// <returns></returns>
        public static MvcHtmlString RouteGuidance<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, string modulName, string modulRemark = null)
        {
            Bootstrap.BootstrapRouteGuidance obj = new Bootstrap.BootstrapRouteGuidance()
            {
                ModulName = modulName,
                ModulRemark = modulRemark,
            };
            var rv = html.InnerHelper.Editor("", "BootstrapRouteGuidance", new { obj });
            return rv;
        }

        public static MvcHtmlString TableFor<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, Expression<Func<TViewModel, IBaseListViewModel<BaseEntity, BaseSearcher>>> fieldExp)
        {
            var v = fieldExp.Compile().Invoke(html.InnerHelper.ViewData.Model);
            BootStrapTable obj = new BootStrapTable();
            obj.TableId = Guid.NewGuid().ToString();
            obj.ColumnsJson = v.GetColumnsJson();
            obj.BootStrapTableColumns = v.GetColumnsObj();
            var rv = html.InnerHelper.Editor("", "BootstrapTable", new { obj });
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
