using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.Attributes
{
    /// <summary>
    /// 标记Controller与Action 名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ControllerOrActionDescriptionAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public ControllerOrActionDescriptionAttribute()
        {
        }
        public ControllerOrActionDescriptionAttribute(string displayName)
        {
            DisplayName = displayName;
        }

    }
}
