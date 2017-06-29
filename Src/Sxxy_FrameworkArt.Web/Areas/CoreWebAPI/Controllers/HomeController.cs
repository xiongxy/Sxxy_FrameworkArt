using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Web.Areas.CoreWebAPI.Controllers
{
    public class HomeController : BaseController
    {
        [HttpPost]
        public ContentResult GetTableData(string viewModelFullName, FormCollection fc)
        {
            var result = new ContentResult();
            var vm = CreateViewModel(VmFullName: viewModelFullName, passInit: true) as IBaseListViewModel<BaseEntity, BaseSearcher>;
            RedoUpdateModel(vm, fc);
            if (vm != null)
            {
                StringBuilder sb = new StringBuilder();
                result.ContentType = "text";
                result.Content = vm.GetDataJson();
            }
            return result;
        }

        [HttpPost]
        public ContentResult GetTreeTableData(string viewModelFullName, FormCollection fc)
        {
            var result = new ContentResult();
            var vm = CreateViewModel(VmFullName: viewModelFullName, passInit: true) as IBaseListViewModel<BaseEntity, BaseSearcher>;
            RedoUpdateModel(vm, fc);
            if (vm != null)
            {
                StringBuilder sb = new StringBuilder();
                result.ContentType = "text";
                result.Content = vm.GetTreeDataJson();
            }
            return result;
        }
    }
}