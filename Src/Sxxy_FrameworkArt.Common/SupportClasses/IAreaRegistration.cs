using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.SupportClasses
{
    interface IAreaRegistration
    {
        string AreaName { get; }
        string AreaRoutePrefix { get; }
    }
}
