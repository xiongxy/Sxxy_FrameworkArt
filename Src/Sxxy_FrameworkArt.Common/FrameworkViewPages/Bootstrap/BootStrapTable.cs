using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap
{
    public class BootStrapTable
    {
        /// <summary>
        /// 表格ID
        /// </summary>
        public string TableId { get; set; }
        /// <summary>
        /// 表格名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 第一次加载是是否执行搜索
        /// </summary>
        public bool IsLoadData { get; set; }
        /// <summary>
        /// 表格针对的viewModel
        /// </summary>
        public string ViewModel { get; set; }
        /// <summary>
        /// 列信息Json格式
        /// </summary>
        public string BootStrapTableColumnsJson { get; set; }
        /// <summary>
        /// 列信息对象格式
        /// </summary>
        public List<BootStrapTableColumn> BootStrapTableColumnsObj { get; set; }
        /// <summary>
        /// 动作信息json格式
        /// </summary>
        public string ActionsJson { get; set; }
        /// <summary>
        /// 搜索信息对象格式
        /// </summary>
        public List<BootStrapTableSearcherFields> BootStrapTableSearcherFieldsObj { get; set; }


    }
    public class BootStrapTreeTable
    {
        /// <summary>
        /// 表格ID
        /// </summary>
        public string TableId { get; set; }
        /// <summary>
        /// 表格名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 第一次加载是是否执行搜索
        /// </summary>
        public bool IsLoadData { get; set; }
        /// <summary>
        /// 表格针对的viewModel
        /// </summary>
        public string ViewModel { get; set; }
        public string FieldsJson { get; set; }
        /// <summary>
        /// 列信息Json格式
        /// </summary>
        public string ColumnsJson { get; set; }
        /// <summary>
        /// 列信息对象格式
        /// </summary>
        public List<BootStrapTableColumn> BootStrapTableColumns { get; set; }
        /// <summary>
        /// 动作信息json格式
        /// </summary>
        public string ActionsJson { get; set; }
        /// <summary>
        /// 搜索字段
        /// </summary>
        public List<BootStrapTableSearcherFields> BootStrapTableSearcherFields { get; set; }
    }
    public class BootStrapTableColumn
    {
        public string Title { get; set; }
    }
    public class BootStrapTableSearcherFields
    {
        public string Title { get; set; }
    }
}
