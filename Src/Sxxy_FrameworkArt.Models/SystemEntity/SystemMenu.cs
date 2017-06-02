using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemMenu : BaseEntity
    {
        public string PageName { get; set; }
        public string ActionName { get; set; }
        public string ModuleName { get; set; }
        public bool? FolderOnly { get; set; }
        public bool? IsInherit { get; set; }
        public List<FunctionPrivilege> Privileges { get; set; }
        public Guid? ActionID { get; set; }
        public Guid? ModuleID { get; set; }
        public Guid? DomainId { get; set; }
        public SystemDomain Domain { get; set; }
        public bool? ShowOnMenu { get; set; }
        public bool? IsPublic { get; set; }
        public int? DisplayOrder { get; set; }

        public bool? IsInside { get; set; }
        public string Url { get; set; }

        #region ITreeData成员

        public long? ParentId { get; set; }
        public SystemMenu Parent { get; set; }
        public List<SystemMenu> Children { get; set; }
        public IEnumerable<SystemMenu> GetChildren()
        {
            return Children == null ? null : Children.AsEnumerable();
        }
        #endregion
    }
}
