using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemUserBase : PersistEntity
    {

        [Display(Name = "编码")]
        public string Code { get; set; }
        [Display(Name = "邮箱")]
        public string Email { get; set; }
        [Display(Name = "姓名")]
        public string Name { get; set; }
        public List<SystemUserAndRoleCorresponding> SystemUserAndRoleCorrespondings { get; set; }
    }
    public class SystemUser : SystemUserBase
    {
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Display(Name = "工作电话")]
        public string WorkPhone { get; set; }
        [Display(Name = "手机电话")]
        public string CellPhone { get; set; }
        [Display(Name = "家庭电话")]
        public string HomePhone { get; set; }
        [Display(Name = "传真号码")]
        public string FaxNum { get; set; }
        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "邮政编码")]
        public string ZipCode { get; set; }
        [Display(Name = "加入工作时间")]
        public DateTime? StartWorkDate { get; set; }
        [Display(Name = "部门编号")]
        public Guid? DepartmentId { get; set; }
        public SystemDepartment Department { get; set; }
    }
}
