using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemMenu : BaseEntity, ITreeData<SystemMenu>
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 是否是文件夹
        /// </summary>
        public bool? FolderOnly { get; set; }
        /// <summary>
        /// 是否继承
        /// </summary>
        public bool? IsInherit { get; set; }
        //public List<FunctionPrivilege> Privileges { get; set; }
        public Guid? ActionId { get; set; }
        public Guid? ModuleId { get; set; }
        public Guid? DomainId { get; set; }
        public SystemDomain Domain { get; set; }
        /// <summary>
        /// 是否显示在菜单上
        /// </summary>
        public bool? ShowOnMenu { get; set; }
        /// <summary>
        /// 是否公开
        /// </summary>
        public bool? IsPublic { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int? DisplayOrder { get; set; }
        /// <summary>
        /// 菜单url
        /// </summary>
        public string Url { get; set; }
        //public bool? IsInside { get; set; }


        #region ITreeData成员

        public Guid? ParentId { get; set; }
        public SystemMenu Parent { get; set; }
        public List<SystemMenu> Children { get; set; }
        public IEnumerable<SystemMenu> GetChildren()
        {
            return Children == null ? null : Children.AsEnumerable();
        }
        #endregion
    }
}
