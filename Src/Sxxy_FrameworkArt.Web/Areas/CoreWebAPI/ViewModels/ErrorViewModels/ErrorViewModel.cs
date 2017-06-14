using System;
using Sxxy_FrameworkArt.Common;

namespace Sxxy_FrameworkArt.Web.Areas.CoreWebAPI.ViewModels.ErrorViewModels
{
    public class ErrorViewModel : BaseViewModel
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public Exception Exception { get; set; }
    }
}