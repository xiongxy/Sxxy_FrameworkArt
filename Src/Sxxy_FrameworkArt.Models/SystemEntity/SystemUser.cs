using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemUser : BaseEntity
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public List<SystemRole> Roles { get; set; }
    }

  
}
