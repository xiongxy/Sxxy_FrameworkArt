using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Web.ViewModels.System.SystemUserViewModels;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    [ControllerOrActionDescription("系统用户管理")]
    public class SystemUserController : BaseController
    {
        // GET: SystemUser
        public ActionResult Index()
        {
            var vm = CreateViewModel<SystemUserListViewModel>();
            return PartialView(vm);
        }
        public ActionResult Create()
        {
            var vm = CreateViewModel<SystemUserViewModel>();
            return PartialView(vm);
        }

        [HttpPost]
        public  ActionResult Create(SystemUserViewModel vm)
        {
            //foreach (var item in vm.AllKeys)
            //{
            //    var vv = vm[item];
            //}
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                //vm.DoAdd();
                return new ContentResult() { Content = "成功！" };
            }
        }
    }
}