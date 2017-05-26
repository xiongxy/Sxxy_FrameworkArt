using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class FunctionPrivilege : BaseEntity
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid MenuItemId { get; set; }
        public SystemMenu MenuItem { get; set; }
        public bool? Allowed { get; set; }
    }
}
