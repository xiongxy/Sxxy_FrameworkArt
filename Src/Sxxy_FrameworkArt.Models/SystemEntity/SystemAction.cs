using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemAction : BaseEntity
    {
        /// <summary>
        /// 动作名称 action 中文名
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 方法名称（action）
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 对应controller
        /// </summary>
        public Guid? ModuleId { get; set; }
        public SystemModule Module { get; set; }
        public string Parameter { get; set; }
        public List<string> ParasToRunTest { get; set; }
    }
}
