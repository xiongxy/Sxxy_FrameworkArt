using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Web.Areas.CoreWebAPI.Controllers
{
    public class HomeController : BaseController
    {
        [HttpPost]
        public ContentResult GetTableData(string viewModelFullName)
        {
            var result = new ContentResult();
            var vm = CreateViewModel(viewModelFullName) as IBaseListViewModel<BaseEntity, BaseSearcher>;
            if (vm != null)
            {
                StringBuilder sb = new StringBuilder();
                result.ContentType = "text";
                result.Content= vm.GetDataHtml();
            }
            return result;
        }
    }
}