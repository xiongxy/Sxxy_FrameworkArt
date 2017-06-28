﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
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
            var rv = html.InnerHelper.Editor("", $"BootstrapRouteGuidance", new { obj });
            return rv;
        }
        /// <summary>
        /// TableFor 生成一个表格给予使用
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="fieldExp"></param>
        /// <param name="isLoadData">第一次是否加载数据，默认是True</param>
        /// <returns></returns>
        public static MvcHtmlString TableFor<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, Expression<Func<TViewModel, IBaseListViewModel<BaseEntity, BaseSearcher>>> fieldExp, bool isLoadData = true)
        {
            var v = fieldExp.Compile().Invoke(html.InnerHelper.ViewData.Model);
            var propertyInfos = v.Searcher.GetType().GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            List<BootStrapTableSearcherFields> list = new List<BootStrapTableSearcherFields>();
            foreach (var item in propertyInfos)
            {
                list.Add(new BootStrapTableSearcherFields() { Title = item.Name });
            }
            BootStrapTable obj = new BootStrapTable();
            obj.TableId = Guid.NewGuid().ToString();
            obj.ColumnsJson = v.GetColumnsJson();
            obj.BootStrapTableColumns = v.GetColumnsObj();
            obj.IsLoadData = isLoadData;
            obj.ViewModel = v.GetType().FullName + "," + v.GetType().Assembly.FullName.Substring(0, v.GetType().Assembly.FullName.IndexOf(",", StringComparison.Ordinal));
            obj.BootStrapTableSearcherFields = list;
            var rv = html.InnerHelper.Editor("", $"BootstrapTable", new { obj });
            return rv;
        }
        /// <summary>
        /// TextField 生成一个带Lable标签的文本框
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="fieldExp"></param>
        /// <param name="labelText">标签名</param>
        /// <param name="inputType">标签类型，默认text</param>
        /// <param name="description">文本框默认内容（虚）</param>
        /// <returns></returns>
        public static MvcHtmlString TextField<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, Expression<Func<TViewModel, object>> fieldExp, string labelText = null, string inputType = "text", string description = null)
        {
            PropertyInfo pi = PropertyHelper.GetPropertyInfo(fieldExp);
            string label = PropertyHelper.GetPropertyDisplayName(pi, labelText);
            BootstrapTextField obj = new BootstrapTextField
            {
                LableText = label,
                InputType = inputType,
                Description = description
            };
            var rv = html.InnerHelper.Editor("", $"BootStrapTextField", new { obj });
            return rv;
        }
        /// <summary>
        /// 生成一个搜索面板，SearcherPanel
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString SearcherPanel<TViewModel>(this BootstrapHtmlHelper<TViewModel> html)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<button type=\"submit\" class=\"btn btn-info pull-right\">Submit</button>");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            return new MvcHtmlString("");
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
