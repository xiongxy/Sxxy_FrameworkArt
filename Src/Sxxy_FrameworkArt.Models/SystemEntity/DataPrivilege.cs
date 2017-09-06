using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class DataPrivilege : BaseEntity
    {
        public Guid? UserId { get; set; }
        public string TableName { get; set; }
        public Guid? RelateId { get; set; }
        public Guid? DomainId { get; set; }
    }
}
