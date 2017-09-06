using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemDepartment : BaseEntity
    {
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public List<SystemUser> Users { get; set; }
    }
}
