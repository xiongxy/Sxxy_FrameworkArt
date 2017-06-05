using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.Attributes
{
    /// <summary>
    /// 标记Controller或Action 不受权限控制，只要登录任何人都可以访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
  public  class AllRightsAttribute : Attribute
    {
    }
}
