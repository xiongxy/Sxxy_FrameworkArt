using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    /// <summary>
    /// 系统角色
    /// </summary>
    public class SystemRole : BaseEntity
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色简介
        /// </summary>
        public string RoleRemark { get; set; }
        //public List<SystemUser> Users { get; set; }
    }
}
