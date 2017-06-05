using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public enum UserTypeEnum { Inside, OutSide }
    public class SystemUserBase : BaseEntity
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public List<SystemRole> Roles { get; set; }
    }

    public class SystemUser : SystemUserBase
    {
        public string Password { get; set; }

        public string WorkPhone { get; set; }

        public string CellPhone { get; set; }

        public string HomePhone { get; set; }

        public string Fax { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public DateTime? StartWorkDate { get; set; }

        public bool IsValid { get; set; }

        //public long? PhotoID { get; set; }

        //public Type Photo { get; set; }

        public long? DepartmentID { get; set; }

        public SystemDepartment Department { get; set; }

        public UserTypeEnum? UserType { get; set; }
    }


}
