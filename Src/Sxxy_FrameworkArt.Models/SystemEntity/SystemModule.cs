using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemModule
    {
        public string ModuleName { get; set; }
        public string ClassName { get; set; }
        public List<SystemAction> Actions { get; set; }

        public Guid? AreaId { get; set; }
        public SystemArea Area { get; set; }
        public string NameSpace { get; set; }
    }
}
