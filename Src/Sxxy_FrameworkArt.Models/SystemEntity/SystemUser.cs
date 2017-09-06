using System;
using System.Collections.Generic;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemUserBase : PersistEntity
    {
        /// <summary>
        /// 编码(用户名)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        public List<SystemRole> Roles { get; set; }
    }

    public class SystemUser : SystemUserBase
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 工作电话
        /// </summary>
        public string WorkPhone { get; set; }
        /// <summary>
        /// 手机电话
        /// </summary>
        public string CellPhone { get; set; }
        /// <summary>
        /// 家庭电话
        /// </summary>
        public string HomePhone { get; set; }
        /// <summary>
        /// 传真号码
        /// </summary>
        public string FaxNum { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 加入工作时间
        /// </summary>
        public DateTime? StartWorkDate { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        public Guid? DepartmentId { get; set; }
        public SystemDepartment Department { get; set; }
    }
}
