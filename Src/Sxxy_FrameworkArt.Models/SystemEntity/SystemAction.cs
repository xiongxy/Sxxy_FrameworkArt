using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemAction : BaseEntity
    {
        public string ActionName { get; set; }
        public string MethodName { get; set; }
        public Guid? ModuleId { get; set; }
        public SystemModule Module { get; set; }
        public string Parameter { get; set; }
        public List<string> ParasToRunTest { get; set; }
    }
}
