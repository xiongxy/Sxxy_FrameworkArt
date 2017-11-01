using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemUserAndRoleCorresponding : BaseEntity
    {
        public Guid SystemRoleId { get; set; }
        public SystemRole SystemRole { get; set; }
        public Guid SystemUserId { get; set; }
        public SystemUser SystemUser { get; set; }
    }
}
