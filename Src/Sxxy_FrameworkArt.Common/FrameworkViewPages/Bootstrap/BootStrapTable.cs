using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap
{
    public class BootStrapTable
    {
        public string TableId { get; set; }
        public string TableName { get; set; }
        public string FieldsJson { get; set; }
        /// <summary>
        /// 列信息Json格式
        /// </summary>
        public string ColumnsJson { get; set; }
        /// <summary>
        /// 列信息对象格式
        /// </summary>
        public List<BootStrapTableColumn> BootStrapTableColumns { get; set; }
        public string ActionsJson { get; set; }
    }

    public class BootStrapTableColumn
    {
        public string Title { get; set; }
    }
}
