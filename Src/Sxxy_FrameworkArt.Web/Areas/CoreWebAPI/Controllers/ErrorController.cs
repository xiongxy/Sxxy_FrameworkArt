using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Web.Areas.CoreWebAPI.ViewModels.ErrorViewModels;

namespace Sxxy_FrameworkArt.Web.Areas.CoreWebAPI.Controllers
{
    [ControllerOrActionDescription("错误提示")]
    public class ErrorController : BaseController, IErrorController
    {
        public ActionResult General(Exception exception, string url, string ip)
        {
            throw new NotImplementedException();
        }

        // GET: CoreWebAPI/Error
        public ActionResult Show(Exception exception)
        {
            if (exception == null)
            {
                exception = new ApplicationException("未知错误");
            }
            var vm = CreateViewModel<ErrorViewModel>(values: x => x.Exception == exception && x.ActionName == "General" && x.ControllerName == "Error");
            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("Error", vm);
            }
            else
            {
                return View("ErrorPage", vm);
            }
        }
    }
}