using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.Attributes
{
    /// <summary>
    /// 标记公共页面，不需要登录即可访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    class PublicAttribute : Attribute
    {
    }
}
