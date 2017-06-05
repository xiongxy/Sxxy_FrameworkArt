using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common.SupportClasses;
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
            set
            {
                _dc = value;
            }
        }



        #endregion
        #region 权限（属性）
        public static List<string> AllRightActions { get; set; }

        public static List<IDataPrivilege> DataPrivileges { get; set; }


        #endregion
        #region  是否开始调试模式(属性)
        public static bool IsQuickDebug
        {
            get
            {
                return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["QuickDebug"]);
            }
        }
        #endregion
        #region 域（属性）
        public static List<SystemDomain> Domains
        {
            get
            {
                if (HttpRuntime.Cache["Domains"] == null)
                {
                    using (DataContext dc = new DataContext())
                    {
                        var data = dc.SystemDomains.ToList();
                        HttpRuntime.Cache.Add("Domains", data, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(365, 0, 0, 0), System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                }
                return HttpRuntime.Cache["Domains"] as List<SystemDomain>;
            }
        }

        public static string MainDomain
        {
            get
            {
                if (HttpRuntime.Cache["MainDomain"] == null)
                {
                    var v = System.Configuration.ConfigurationManager.AppSettings["MainDomain"];
                    string md = "";
                    if (!string.IsNullOrEmpty(v))
                    {
                        md = v;
                    }
                    HttpRuntime.Cache.Add("MainDomain", md, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(365, 0, 0, 0), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                return HttpRuntime.Cache["MainDomain"].ToString();
            }
        }

        public static long? DomainID { get; set; }

        #endregion
        #region 菜单 （属性）
        public static List<SystemMenu> SystemMenuProperty
        {
            get
            {
                if (HttpRuntime.Cache["SystemMenuProperty"] == null)
                {
                    List<SystemMenu> data = null;
                    if (!string.IsNullOrEmpty(MainDomain))
                    {
                        if (DomainID != null)
                        {

                        }
                    }
                    else
                    {
                        using (DataContext dc = new DataContext())
                        {
                            data = dc.SystemMenus
                                .OrderBy(x => x.DisplayOrder)
                                .ToList();
                        }
                        HttpRuntime.Cache.Add("SystemMenuProperty", data, null, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                }
                return HttpRuntime.Cache["SystemMenuProperty"] == null ? new List<SystemMenu>() : HttpRuntime.Cache["SystemMenuProperty"] as List<SystemMenu>;
            }
        }



        #endregion


        //T CreateViewModel<T>(long? id = null, IEnumerable<long> IDs = null, Expression<Func<T, object>> values = null,
        //    bool passInit = false) where T : BaseViewModel
        //{
        //    SetValuesParser p = new SetValuesParser();
        //    var dir = p.Parse(values);
        //    return CreateViewModel(typeof(T), id, IDs, dir, passInit || IDs != null) as T;
        //}

        //BaseViewModel<T> CreateViewModel(Type type, long? id = null, IEnumerable<long> ids = null,
        //    Dictionary<string, object> value = null, bool passInt = false)
        //{

        //}
    }
}
