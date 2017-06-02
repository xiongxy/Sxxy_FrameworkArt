using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemRole : BaseEntity
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleRemark { get; set; }
        //public List<SystemUser> Users { get; set; }
    }
}
