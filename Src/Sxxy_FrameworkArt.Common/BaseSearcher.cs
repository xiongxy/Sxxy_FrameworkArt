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
        public int Draw { get; set; }
        /// <summary>
        /// 开始行数
        /// </summary>
        public int StartRow { get; set; }
        /// <summary>
        /// 该值获取或设置数据呈现控件每次要显示数据表中的的数据的项数
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalRecords { get; set; }
    }
}
