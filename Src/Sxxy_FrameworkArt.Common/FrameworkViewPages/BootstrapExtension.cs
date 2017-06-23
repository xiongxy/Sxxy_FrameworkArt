using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages
{
    public static class BootstrapExtension
    {
        #region 页面布局
        /// <summary>
        /// 页面布局-栅格系统(行)
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static BootStrapRow PageLayout_BeginRow<TViewModel>(this BootstrapHtmlHelper<TViewModel> html)
        {
            StringBuilder sb = new StringBuilder();
            var vm = html.InnerHelper.ViewData.Model as BaseViewModel;
            sb.Append("<div class=\"row\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapRow mf = new BootStrapRow(html.InnerHelper.ViewContext);
            return mf;
        }
        /// <summary>
        /// 页面布局-栅格系统(列)
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="widthPlaceholder">宽度占位符(1-12),默认为12</param>
        /// <returns></returns>
        public static BootStrapRow PageLayout_BeginColumn<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, int widthPlaceholder = 12)
        {
            StringBuilder sb = new StringBuilder();
            var vm = html.InnerHelper.ViewData.Model as BaseViewModel;
            sb.Append("<div class=\"col-md-" + widthPlaceholder + "\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapRow mf = new BootStrapRow(html.InnerHelper.ViewContext);
            return mf;
        }
        /// <summary>
        /// 页面布局-Box(盒子)
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="layout">Box布局，类型BootStarpBoxLayout枚举</param>
        /// <returns></returns>
        public static BootStrapBox PageLayout_BeginBox<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, BootStarpBoxLayout layout = BootStarpBoxLayout.Norml)
        {
            var atr = "";
            if (layout != BootStarpBoxLayout.Norml)
            {
                atr = "-" + layout.ToString().ToLower();
            }
            StringBuilder sb = new StringBuilder();
            var vm = html.InnerHelper.ViewData.Model as BaseViewModel;
            sb.Append("<div class=\"box" + atr + "\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapBox mf = new BootStrapBox(html.InnerHelper.ViewContext);
            return mf;
        }
        #endregion
        /// <summary>
        /// 表单开始
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="title">表单页面显示标题,默认为无</param>
        /// <returns></returns>
        public static BootStrapForm BeginForm<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, string title = "")
        {
            StringBuilder sb = new StringBuilder();
            var vm = html.InnerHelper.ViewData.Model as BaseViewModel;
            var formTitle = "";//表单名称
            sb.Append("<div class=\"box box-primary\">");
            sb.Append("<div class=\"box-header with-border\">");
            sb.Append("<h3 class=\"box-title\">" + formTitle + "</h3>");
            sb.Append("</div>");
            sb.Append("<form role=\"form\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapForm mf = new BootStrapForm(html.InnerHelper.ViewContext);
            return mf;
        }
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
        /// <summary>
        /// TableFor 生成一个表格给予使用
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="fieldExp"></param>
        /// <returns></returns>
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

        public static MvcHtmlString TextField<TViewModel>(
            this BootstrapHtmlHelper<TViewModel> html,
            Expression<Func<TViewModel, object>> fieldExp,
            string labelText = null,
            string inputType = "text",
            string description = null)
        {
            PropertyInfo pi = PropertyHelper.GetPropertyInfo(fieldExp);
            string label = PropertyHelper.GetPropertyDisplayName(pi, labelText);
            BootstrapTextField obj = new BootstrapTextField();
            obj.LableText = label;
            obj.InputType = inputType;
            obj.Description = description;
            var rv = html.InnerHelper.Editor("", "BootStrapTextField", new { obj });
            return rv;
        }

    }
    public enum BootStarpBoxLayout
    {
        Norml,
        Header,
        Body,
        Footer,
    }
}
