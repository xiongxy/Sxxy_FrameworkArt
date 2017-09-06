using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    /// <summary>
    /// 系统模块(同等于Controller)
    /// </summary>
    public class SystemModule : BaseEntity
    {
        /// <summary>
        /// 模块名称 Controller 中文名
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// Controller 类名
        /// </summary>
        public string ClassName { get; set; }
        public List<SystemAction> Actions { get; set; }
        public Guid? AreaId { get; set; }
        public SystemArea Area { get; set; }
        public string NameSpace { get; set; }
    }
}
