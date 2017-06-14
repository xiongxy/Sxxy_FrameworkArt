using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sxxy_FrameworkArt.Common
{
    public interface IErrorController : IController
    {
        ActionResult General(Exception exception, string url, string ip);
        ActionResult Show(Exception exception);
    }
}
