using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemArea : BaseEntity
    {
        public string AreaName { get; set; }

        public string Prefix { get; set; }
        public List<SystemModule> Modules { get; set; }
    }
}
