using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Newtonsoft.Json;
using Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;
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
            BootStrapRow br = new BootStrapRow(html.InnerHelper.ViewContext);
            return br;
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
            BootStrapRow br = new BootStrapRow(html.InnerHelper.ViewContext);
            return br;
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
            BootStrapBox bb = new BootStrapBox(html.InnerHelper.ViewContext);
            return bb;
        }
        #endregion

        /// <summary>
        /// 搜索面板（表单）
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="fieldExp"></param>
        /// <param name="viewModelId"></param>
        /// <param name="title">表单页面显示标题,默认为无</param>
        /// <param name="showSearch">是否展现搜索功能</param>
        /// <param name="dataTableJsName"></param>
        /// <returns></returns>
        public static BootStrapSearcherPanel SearcherPanel<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, Expression<Func<TViewModel, IBaseListViewModel<BaseEntity, BaseSearcher>>> fieldExp, Guid viewModelId, string title = "", bool showSearch = true, string dataTableJsName = "objTable")
        {
            StringBuilder sb = new StringBuilder();
            var vm = fieldExp.Compile().Invoke(html.InnerHelper.ViewData.Model);
            sb.Append($"<div id=\"searcherPanel_{viewModelId}\" class=\"box box-danger\">");//.box默认有一个顶部颜色，与 .box-default（灰色）相同，.box-success（绿色），.box-warning（黄色），.box-danger（红色）等可覆盖默认的颜色样式
            if (title != "")
            {
                sb.Append("<div class=\"box-header with-border titlebar\">");
                sb.Append("<h3 class=\"box-title\">" + title + "</h3>");
                sb.Append("</div>");
            }
            if (showSearch)
            {
                sb.Append("<div class=\"box-header with-border actionbar\">");
                sb.Append($"<a actiontype=\"search\" class=\"btn btn-app\" onclick=\"javascript:{dataTableJsName}.ajax.reload();\">");
                sb.Append("<i class=\"fa fa-search\"></i>search</a>");
                foreach (var item in vm.GridActions)
                {
                    sb.Append($"<a action-type=\"{item.ActionName}\" class=\"btn btn-app\" modalId=\"#modal_{viewModelId}\" action-url=\"{item.Url}\">");
                    sb.Append("<i class=\"" + item.IconCls + "\"></i>" + item.Name + "</a>");
                }
                sb.Append("</div>");
                sb.Append($"<script>SxxyJs.BootStrapSearcherPanel(\"#searcherPanel_{viewModelId}\",\"{dataTableJsName}\");</script>");
            }
            sb.Append("<div class=\"box-body\">");
            sb.Append("<div class=\"col-md-12\">");
            sb.Append("<form class=\"form-inline\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapSearcherPanel mp = new BootStrapSearcherPanel(html.InnerHelper.ViewContext);
            return mp;
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
        /// <param name="isEnableCheckBox">是否启用checkbox</param>
        /// <param name="jsObjName">JS对象的名称，通常为默认</param>
        /// <param name="isLoadData">第一次是否加载数据，默认是True</param>
        /// <returns></returns>
        public static MvcHtmlString TableFor<TViewModel>(this BootstrapHtmlHelper<TViewModel> html,
                                                                        Expression<Func<TViewModel,
                                           IBaseListViewModel<BaseEntity, BaseSearcher>>> fieldExp,
                                                                           bool isEnableCheckBox = true,
                                                                     string jsObjName = "objTable",
                                                                            bool isLoadData = true)
        {
            var v = fieldExp.Compile().Invoke(html.InnerHelper.ViewData.Model);
            var propertyInfos = v.Searcher.GetType().GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            List<BootStrapTableSearcherFields> list = new List<BootStrapTableSearcherFields>();
            foreach (var item in propertyInfos)
            {
                list.Add(new BootStrapTableSearcherFields() { Title = item.Name });
            }
            BootStrapTable obj = new BootStrapTable();
            obj.TableId = (html.InnerHelper.ViewData.Model as BaseViewModel).ViewModelId.ToString();
            obj.BootStrapTableColumnsJson = v.GetColumnsJson();
            List<BootStrapTableColumn> tableColumns = new List<BootStrapTableColumn>() { };
            tableColumns.AddRange(v.GetColumnsObj());
            obj.IsEnableCheckBox = isEnableCheckBox;
            obj.BootStrapTableColumnsObj = tableColumns;
            obj.ActionsJson = v.GetActionJson();
            obj.JsObjName = jsObjName;
            obj.IsLoadData = isLoadData;
            obj.ViewModel = v.GetType().FullName + "," + v.GetType().Assembly.FullName.Substring(0, v.GetType().Assembly.FullName.IndexOf(",", StringComparison.Ordinal));
            obj.BootStrapTableSearcherFieldsObj = list;
            var rv = html.InnerHelper.Editor("", $"BootstrapTable", new { obj });
            return rv;
        }

        public static MvcHtmlString SimpleTableFor<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, Expression<Func<TViewModel, object>> fieldExp)
        {
            BootStrapSimpleTable bootStrapSimpleTable = new BootStrapSimpleTable();
            var obj = PropertyHelper.GetPropertyValueList(fieldExp, html.InnerHelper.ViewData.Model);
            bootStrapSimpleTable.DataSourcesJson = JsonConvert.SerializeObject(obj);
            var rv = html.InnerHelper.Editor("", $"BootstrapSimpleTable", new { bootStrapSimpleTable });
            return rv;
        }
        /// <summary>
        /// TreeTableFor 生成一个树形表格给予使用
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="fieldExp"></param>
        /// <param name="isLoadData">第一次是否加载数据，默认是True</param>
        /// <returns></returns>
        public static MvcHtmlString TreeTableFor<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, Expression<Func<TViewModel, IBaseListViewModel<BaseEntity, BaseSearcher>>> fieldExp, bool isLoadData = true)
        {
            var v = fieldExp.Compile().Invoke(html.InnerHelper.ViewData.Model);
            var propertyInfos = v.Searcher.GetType().GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            List<BootStrapTableSearcherFields> list = new List<BootStrapTableSearcherFields>();
            foreach (var item in propertyInfos)
            {
                list.Add(new BootStrapTableSearcherFields() { Title = item.Name });
            }
            BootStrapTreeTable obj = new BootStrapTreeTable();
            obj.TableId = (html.InnerHelper.ViewData.Model as BaseViewModel).ViewModelId.ToString();
            obj.ColumnsJson = v.GetColumnsJson();
            obj.BootStrapTableColumns = v.GetColumnsObj();
            obj.IsLoadData = isLoadData;
            obj.ViewModel = v.GetType().FullName + "," + v.GetType().Assembly.FullName.Substring(0, v.GetType().Assembly.FullName.IndexOf(",", StringComparison.Ordinal));
            obj.BootStrapTableSearcherFields = list;
            var rv = html.InnerHelper.Editor("", $"BootstrapTreeTable", new { obj });
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
            var vm = html.InnerHelper.ViewData.Model as BaseViewModel;
            string label = PropertyHelper.GetPropertyDisplayName(pi, labelText);
            string name = PropertyHelper.GetPropertyName(fieldExp);
            string error = PropertyHelper.GetPropertyErrors(html.InnerHelper, fieldExp);
            object obj = new object();
            string value = PropertyHelper.GetPropertyValue(fieldExp, html.InnerHelper.ViewData.Model);
            BootstrapTextField bootstrapTextField = new BootstrapTextField
            {
                LableText = label,
                InputType = inputType,
                Description = description,
                InputId = label + "txt" + "_" + (vm == null ? "" : vm.ViewModelId.ToString()),
                InputName = name,
                Error = error,
                Value = value,
            };
            var rv = html.InnerHelper.Editor("", $"BootStrapTextField", new { bootstrapTextField });
            return rv;
        }
        /// <summary>
        /// 生成一个Button按钮
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="buttonName"></param>
        /// <param name="buttonType">button|reset|submit</param>
        /// <returns></returns>
        public static MvcHtmlString Button<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, string buttonName, string buttonType = "button")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<button type=\"{buttonType}\" class=\"btn btn-info pull-right\">{buttonName}</button>");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            return new MvcHtmlString("");
        }

        public static MvcHtmlString Select2<TViewModel, TValue>(this BootstrapHtmlHelper<TViewModel> html, Expression<Func<TViewModel, IList<TValue>>> fieldExp, Expression<Func<TViewModel, IEnumerable<SimpleSelectItem>>> allItems)
        {
            var list = fieldExp.Compile().Invoke(html.InnerHelper.ViewData.Model);
            var ai = allItems.Compile().Invoke(html.InnerHelper.ViewData.Model);
            List<BootStrapSelect2Item> bootStrapSelect2Item = new List<BootStrapSelect2Item>();
            foreach (var simpleSelectItem in ai)
            {
                bootStrapSelect2Item.Add(new BootStrapSelect2Item() { id = simpleSelectItem.Value.ToString(), text = simpleSelectItem.Text.ToString() });
            }
            BootStrapSelect2 bootStrapSelect2 = new BootStrapSelect2
            {
                JsonDataBySelect2 = JsonConvert.SerializeObject(bootStrapSelect2Item),
                SelectedItem = JsonConvert.SerializeObject(list)
            };
            var rv = html.InnerHelper.Editor("", $"BootStrapSelect2", new { bootStrapSelect2 });
            return rv;
        }

        /// <summary>
        /// BootStrapForm 
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="formAction"></param>
        /// <param name="formId"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static BootStrapForm BootStrapForm<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, string formAction, string formId = "myForm", string method = "Get")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<form class=\"form-inline\" id=\"{formId}\" action=\"{formAction}\" method=\"{method}\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapForm bootStrapForm = new BootStrapForm(html.InnerHelper.ViewContext);
            return bootStrapForm;
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
        #region 模态框
        public static BootStrapModal ModalWindowDialog<TViewModel>(this BootstrapHtmlHelper<TViewModel> html, Guid vmGuid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<div id=\"modaldialog_{vmGuid}\" class=\"modal-dialog\" role=\"document\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapModal bootStrapModal = new BootStrapModal(html.InnerHelper.ViewContext);
            return bootStrapModal;
        }
        public static BootStrapModal ModalWindowContent<TViewModel>(this BootstrapHtmlHelper<TViewModel> html)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"modal-content\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapModal bootStrapModal = new BootStrapModal(html.InnerHelper.ViewContext);
            return bootStrapModal;
        }
        public static BootStrapModal ModalWindowHeader<TViewModel>(this BootstrapHtmlHelper<TViewModel> html)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"modal-header\">");
            sb.Append("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapModal bootStrapModal = new BootStrapModal(html.InnerHelper.ViewContext);
            return bootStrapModal;
        }
        public static BootStrapModal ModalWindowBody<TViewModel>(this BootstrapHtmlHelper<TViewModel> html)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"modal-body\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapModal bootStrapModal = new BootStrapModal(html.InnerHelper.ViewContext);
            return bootStrapModal;
        }
        public static BootStrapModal ModalWindowFooter<TViewModel>(this BootstrapHtmlHelper<TViewModel> html)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"modal-footer\">");
            html.InnerHelper.ViewContext.Writer.WriteLine(sb.ToString());
            BootStrapModal bootStrapModal = new BootStrapModal(html.InnerHelper.ViewContext);
            return bootStrapModal;
        }
        #endregion
    }
    public enum BootStarpBoxLayout
    {
        Norml,
        Header,
        Body,
        Footer,
    }
}
