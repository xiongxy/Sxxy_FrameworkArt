using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common
{
    public class BaseSearcher : BaseViewModel
    {
        /// <summary>
        /// 调用次数
        /// </summary>
        public static int draw { get; set; }
        /// <summary>
        /// 开始行数
        /// </summary>
        public static int start { get; set; }
        /// <summary>
        /// 显示行数
        /// </summary>
        public static int length { get; set; }
    }
}
