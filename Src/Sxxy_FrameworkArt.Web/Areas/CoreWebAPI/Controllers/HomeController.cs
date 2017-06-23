using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using Sxxy_FrameworkArt.Common;

namespace Sxxy_FrameworkArt.Web.Areas.CoreWebAPI.Controllers
{
    public class HomeController : BaseController
    {
        [HttpPost]
        public ContentResult GetTableData(string viewModelFullName)
        {
            var result = new ContentResult();
            var vm = CreateViewModel("Sxxy_FrameworkArt.Web.ViewModels.System.SystemUserViewModels.SystemUserListViewModel,Sxxy_FrameworkArt.Web");
            if (vm != null)
            {

            }
            return result;
        }
    }
}