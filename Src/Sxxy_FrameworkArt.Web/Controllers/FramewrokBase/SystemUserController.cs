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
            vm.Entity.Address = "bbbb";
            return PartialView(vm);
        }
        [HttpPost]
        public ActionResult Create(SystemUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoAdd();
                return CloseAndRefreshResult();
            }
        }

        public ActionResult Edit(Guid id)
        {
            var vm = CreateViewModel<SystemUserViewModel>(id);
            vm.Entity.Address = "bbbb";
            return PartialView(vm);
        }
        [HttpPost]
        public ActionResult Edit(SystemUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoEdit();
                return CloseAndRefreshResult();
            }
        }

        public ActionResult Delete(Guid[] ids)
        {
            var vm = CreateViewModel<SystemUserViewModel>(IDs: ids);
            return PartialView(vm);
        }
        [HttpPost]
        public ActionResult Delete(SystemUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoDelete();
                return CloseAndRefreshResult();
            }
        }
    }
}