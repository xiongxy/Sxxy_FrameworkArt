using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Common
{
    public class BaseController : AsyncController
    {
        #region 数据库环境

        public static Type DefaultDataContext
        {
            get
            {
                if (HttpRuntime.Cache["DefaultDataContext"] == null)
                {
                    var ls = System.Configuration.ConfigurationManager.AppSettings["DefaultDataContext"];
                    Type type = null;
                    if (!string.IsNullOrEmpty(ls))
                    {
                        type = Type.GetType(ls);
                    }
                    if (type != null)
                    {
                        HttpRuntime.Cache.Add("DefaultDataContext", type, null,
                            System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(365, 0, 0, 0),
                            System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                    else
                    {
                        return null;
                    }
                }
                return HttpRuntime.Cache["DefaultDataContext"] as Type;
            }
        }

        private IDataContext _dc;

        public IDataContext Dc
        {
            get
            {
                if (_dc == null)
                {
                    var constructorInfo = DefaultDataContext.GetConstructor(Type.EmptyTypes);
                    if (constructorInfo != null)
                        _dc = (IDataContext)constructorInfo.Invoke(null);
                }
                return _dc;
            }
            set { _dc = value; }
        }



        #endregion

        #region 权限（属性）

        public static List<string> AllRightActions { get; set; }

        public static List<IDataPrivilege> DataPrivileges { get; set; }


        #endregion

        #region  是否开始调试模式(属性)

        public static bool IsQuickDebug
        {
            get { return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["QuickDebug"]); }
        }

        #endregion

        #region CookiePre（属性）
        public static string CookiePre
        {
            get
            {

                if (HttpRuntime.Cache["CookiePre"] == null)
                {
                    var ls = System.Configuration.ConfigurationManager.AppSettings["CookiePre"];
                    if (string.IsNullOrEmpty(ls))
                    {
                        ls = "SxxyFramework";
                    }
                    HttpRuntime.Cache.Add("CookiePre", ls, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(365, 0, 0, 0), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                return HttpRuntime.Cache["CookiePre"].ToString();
            }
        }
        #endregion

        #region 菜单 （属性）

        public static List<SystemMenu> SystemMenuProperty
        {
            get
            {
                if (HttpRuntime.Cache["SystemMenuProperty"] == null)
                {
                    List<SystemMenu> data = null;
                    using (DataContext dc = new DataContext())
                    {
                        data = dc.SystemMenus
                            .OrderBy(x => x.DisplayOrder)
                            .ToList();
                    }
                    HttpRuntime.Cache.Add("SystemMenuProperty", data, null, DateTime.Now.AddHours(12),
                        System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal,
                        null);
                }
                return HttpRuntime.Cache["SystemMenuProperty"] == null
                    ? new List<SystemMenu>()
                    : HttpRuntime.Cache["SystemMenuProperty"] as List<SystemMenu>;
            }
        }



        #endregion

        #region 当前用户（属性）
        public LoginUserInfo LoginUserInfo
        {
            get
            {
                if (Session == null || Session["UserInfo"] == null)
                {
                    return null;
                }
                else
                {
                    return Session["UserInfo"] as LoginUserInfo;
                }
            }
            set
            {
                Session["UserInfo"] = value;
            }
        }
        #endregion

        public NowControllerInfo NowController { get; set; }

        protected BaseViewModel CreateViewModel(string VmFullName, long? ID = null, IEnumerable<long> IDs = null, bool passInit = false)
        {
            return CreateViewModel(Type.GetType(VmFullName), ID, IDs, null, passInit || IDs != null);
        }
        protected T CreateViewModel<T>(long? ID = null, IEnumerable<long> IDs = null, Expression<Func<T, object>> values = null, bool passInit = false) where T : BaseViewModel
        {
            SetValuesParser p = new SetValuesParser();
            var dir = p.Parse(values);
            return CreateViewModel(typeof(T), ID, IDs, dir, passInit || IDs != null) as T;
        }

        public BaseViewModel CreateViewModel(Type ViewModelType, long? ID = null, IEnumerable<long> IDs = null,
            Dictionary<string, object> values = null, bool passInit = false)
        {
            var ctor = ViewModelType.GetConstructor(Type.EmptyTypes);
            BaseViewModel rv = ctor.Invoke(null) as BaseViewModel;

            var v = Activator.CreateInstance(ViewModelType);
            BaseViewModel baseViewModel = (BaseViewModel)v;
            baseViewModel.Dc = this.Dc;
            baseViewModel.ModelStateDictionarys = this.ModelState;
            baseViewModel.InSideFormCollection = HttpContext == null
                ? new FormCollection()
                : new FormCollection(this.HttpContext.Request.Form);
            baseViewModel.NowController = this.NowController;
            //如果当前的viewModel 继承的是IBaseListViewModel，则初始化Searcher并调用Searcher的InitVM方法
            if (baseViewModel is IBaseListViewModel<BaseEntity, BaseSearcher>)
            {
                var vv = (baseViewModel as IBaseListViewModel<BaseEntity, BaseSearcher>).Searcher;
                BaseSearcher baseSearcher = (baseViewModel as IBaseListViewModel<BaseEntity, BaseSearcher>).Searcher;
                baseSearcher.CopyContext(baseViewModel);
                baseSearcher.DoInit();
            }
            baseViewModel.DoInit();
            return baseViewModel;
        }

        #region  执行
        // 在调用操作方法前调用
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //获取当前访问的模块记录到BaseController
            var vvv = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType;
            object[] objs = vvv.GetCustomAttributes(typeof(ControllerOrActionDescriptionAttribute), true);
            if (objs.Length > 0)
            {
                NowController = new NowControllerInfo
                {
                    NowControllerName = (objs[0] as ControllerOrActionDescriptionAttribute).DisplayName,
                    NowControllerRemark = (objs[0] as ControllerOrActionDescriptionAttribute).Remark
                };
            }
            if (LoginUserInfo == null)
            {
                //判断当前访问路由是否标记公共标签
                var isStaticPublic = filterContext.ActionDescriptor.IsDefined(typeof(PublicAttribute), false) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(PublicAttribute), false);
                if (isStaticPublic == false)
                {
                    var isPublic = SystemMenuProperty
                        .FirstOrDefault(x => x.Url != null
                                    && x.Url.ToLower() == "/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower() + "/" + filterContext.ActionDescriptor.ActionName
                                    && x.IsPublic == true);
                    if (isPublic == null)
                    {
                        try
                        {
                            //HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get(CookiePre + "FFLastPage");
                            //string url = "";
                            //if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                            //{
                            //    url = MainDomain + "/#/Login/Index?rd=" + HttpUtility.UrlEncode("/#" + filterContext.HttpContext.Request.Url.LocalPath.ToString());
                            //}
                            //else
                            //{
                            //    url = MainDomain + "/#/Login/Index?rd=" + HttpUtility.UrlEncode("/#" + cookie.Value);
                            //}
                            var url = "/Login/Index?rd=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.LocalPath);
                            filterContext.Result = Content("<script>window.top.location.href = '" + url + "';</script>");
                        }
                        catch { }
                        return;
                    }
                }
            }
            //foreach (var item in filterContext.ActionParameters)
            //{
            //if (item.Value is BaseViewModel)
            //{
            //    var model = item.Value as BaseViewModel;
            //    model.Session = this.Session;
            //    model.Dc = this._dc;
            //    model.ModelStateDictionarys = this.ModelState;
            //    //model.Cache = HttpRuntime.Cache;
            //    model.InSideFormCollection = new FormCollection(this.HttpContext.Request.Form);
            //如果ViewModel T继承自IBaseBatchVM<BaseVM>，则自动为其中的ListVM和EditModel初始化数据
            //if (model is IBaseBatchVM<BaseVM>)
            //{
            //    var temp = model as IBaseBatchVM<BaseVM>;
            //    if (temp.ListVM != null)
            //    {
            //        temp.ListVM.CopyContext(model);
            //        temp.ListVM.IDs = temp.IDs == null ? new List<long>() : temp.IDs.ToList();
            //        temp.ListVM.SearcherMode = ListVMSearchModeEnum.Batch;
            //    }
            //    if (temp.LinkedVM != null)
            //    {
            //        temp.LinkedVM.CopyContext(model);
            //    }
            //    if (temp.ListVM != null)
            //    {
            //        temp.ListVM.GridActions = new List<GridAction>();
            //        //绑定ListVM的OnAfterInitList事件，当ListVM的InitList完成时，自动将操作列移除
            //        temp.ListVM.OnAfterInitList += (self) =>
            //        {
            //            self.RemoveActionColumn();
            //            if (temp.ErrorMessage.Count > 0)
            //            {
            //                self.AddErrorColumn((item2, value) => { return temp.ErrorMessage[item2.ID]; });
            //            }
            //        };
            //        if (temp.ListVM.Searcher != null)
            //        {
            //            BaseSearcher searcher = temp.ListVM.Searcher;
            //            searcher.CopyContext(model);
            //            //searcher.DoReInit();
            //        }
            //    }
            //}
            //if (model is IBaseMasterDetailsVM<IBaseCRUDVM<BasePoco>, IBasePagedListVM<BasePoco, BaseSearcher>>)
            //{
            //    var temp = model as IBaseMasterDetailsVM<IBaseCRUDVM<BasePoco>, IBasePagedListVM<BasePoco, BaseSearcher>>;
            //    if (temp.ListVM != null)
            //    {
            //        temp.ListVM.CopyContext(model);
            //        temp.ListVM.SearcherMode = ListVMSearchModeEnum.MasterDetail;
            //        if (temp.ListVM.Searcher != null)
            //        {
            //            BaseSearcher searcher = temp.ListVM.Searcher;
            //            searcher.CopyContext(model);
            //            //searcher.DoReInit();
            //        }
            //    }
            //    if (temp.MasterVM != null)
            //    {
            //        temp.MasterVM.CopyContext(model);
            //    }
            //}
            //if (model is IBasePagedListVM<BasePoco, BaseSearcher>)
            //{
            //    BaseSearcher searcher = (model as IBasePagedListVM<BasePoco, BaseSearcher>).Searcher;
            //    searcher.CopyContext(model);
            //    if (filterContext.HttpContext.Request.HttpMethod == "POST")
            //    {
            //        if (Request.Form["SaveInCookie"] != null && Request.Form["SaveInCookie"].ToLower() == "true")
            //        {
            //            Type searcherType = searcher.GetType();
            //            var pros = searcherType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ToList();
            //            pros.Add(searcherType.GetProperty("IsValid"));
            //            foreach (var pro in pros)
            //            {
            //                var propertyType = pro.PropertyType;
            //                object val = pro.GetValue(searcher);
            //                string name = CookiePre + "`Searcher" + "`" + model.VMFullName + "`" + pro.Name;
            //                string valText = "`";
            //                if (val != null)
            //                {
            //                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
            //                    {
            //                        valText = "`" + (val as IList).ToSpratedString() + "`";
            //                    }
            //                    else
            //                    {
            //                        if (val.ToString() != "")
            //                        {
            //                            valText = val.ToString();
            //                        }
            //                    }
            //                }
            //                HttpCookie cookie = new HttpCookie(name, valText);
            //                cookie.Expires = DateTime.Today.AddYears(10);
            //                Response.Cookies.Add(cookie);
            //            }
            //        }
            //        searcher.IsPostBack = true;
            //    }
            //    searcher.DoReInit();
            //}
            //if (model is IBaseImport<BaseTemplateVM>)
            //{
            //    var template = (model as IBaseImport<BaseTemplateVM>).Template;
            //    template.CopyContext(model);
            //}
            //SetReInit(ModelState, model);
            //}
            //}
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //filterContext.HttpContext.Response.Write("<div id=\"vvvvvv\" style=\"height:100%;\" class=\"x-hide-display\"></div>");
            if (filterContext.Result is PartialViewResult || filterContext.ActionDescriptor.ActionName == "Test")
            {
                var isPublic = filterContext.ActionDescriptor.IsDefined(typeof(PublicAttribute), false) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(PublicAttribute), false);
                string script = "";
                string url = "";
                url = "/#/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName;
                //script = @"
                //<script>
                //    var url = '" + url + @"';
                //    if (typeof mainwindow == 'undefined' && typeof popupwindow == 'undefined')
                //    { 
                //        window.location.href = url;
                //    }
                //</script>
                //";
                filterContext.HttpContext.Response.Write(script);
                BaseViewModel viewModel = null;
                if (filterContext.Result is PartialViewResult)
                {
                    viewModel = (filterContext.Result as PartialViewResult).ViewData.Model as BaseViewModel;
                }
                //自动为所有PartialView加上最外层的Div
                if (viewModel != null)
                {
                    //如果Action中没有标记PureHtml，则自动调用FF_InitView方法来自动加入主Panel
                    //如果标记了PureHtml则只填加Div，不做Extjs的其它处理。如果标记了PureHtml的方法中报错，框架会转向/Error/Show方法，而这个方法中是需要Extjs处理的，所以也需要判断
                    //if (filterContext.ActionDescriptor.IsDefined(typeof(PureHtmlAttribute), false) == false || (filterContext.Result is PartialViewResult && ((PartialViewResult)filterContext.Result).Model is ErrorVM))
                    //{
                    //    filterContext.HttpContext.Response.Write("<div id=\"" + model.ViewDivID + "\" style=\"height:100%;\" class=\"x-hide-display\">");
                    //    filterContext.HttpContext.Response.Write(string.Format("<script>FF_InitView('{0}');</script>" + Environment.NewLine, model == null ? "" : model.ViewDivID));
                    //}
                    //else
                    //{
                    //    filterContext.HttpContext.Response.Write("<div id=\"" + model.ViewDivID + "\" style=\"height:100%;\">");
                    //}
                }
            }
            base.OnActionExecuted(filterContext);
        }

        #endregion

        #region 更新model
        /// <summary>
        /// 模拟MVC将FormCollection的值赋给ViewModel的相应字段的过程
        /// </summary>
        /// <param name="vm">ViewModel</param>
        /// <param name="fc">FormCollection</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool RedoUpdateModel(object vm, FormCollection fc, string prefix = null)
        {
            try
            {
                //获取viewmodel的类型
                Type vmType = vm.GetType();
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                //循环FormCollection
                foreach (var item in fc.AllKeys)
                {
                    if (item == "searcher")
                    {
                        var jsonobj = JsonConvert.DeserializeObject(fc[item]);
                        IEnumerable<Newtonsoft.Json.Linq.JProperty> properties = ((Newtonsoft.Json.Linq.JObject)jsonobj).Properties();
                        foreach (var itemProperty in properties)
                        {
                            dictionary.Add("Searcher." + itemProperty.Name, itemProperty.Value.ToString());
                        }
                    }
                    else
                    {
                        PropertyHelper.SetPropertyValue(vm, item, fc.GetValue(item).RawValue, true);
                    }
                }
                foreach (var item in dictionary)
                {
                    PropertyHelper.SetPropertyValue(vm, item.Key, item.Value, true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        protected ContentResult CloseAndRefreshResult(Guid? vmID, string alertMessage = null)
        {
            ContentResult contentResult = new ContentResult();
            contentResult.Content += $"<script>$('#modal_{vmID}').modal('hide');</script>";
            return contentResult;
        }

    }
}
