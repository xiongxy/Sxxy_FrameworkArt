using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.SupportClasses
{
    public class TreeItem
    {
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Activate { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Expand { get; set; }
        /// <summary>
        /// 图标css
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 是否是目录
        /// </summary>
        public bool IsFolder { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool Select { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 跳转的url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 其他信息
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 域ID，用于菜单
        /// </summary>
        public string Domainid { get; set; }
        /// <summary>
        /// 子项
        /// </summary>
        public List<TreeItem> ChildrensItems { get; set; }
    }
    public class TopMenuItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        //public string handler { get; set; }
        public List<TopMenuItem> menu { get; set; }
    }
}
