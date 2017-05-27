using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common.SupportClasses;

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
    }
}
