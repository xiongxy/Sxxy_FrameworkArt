using System;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Web.ViewModels.System.SystemRoleViewModels;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    [ControllerOrActionDescription("系统角色管理")]
    public class SystemRoleController : BaseController
    {
        [ControllerOrActionDescription("查询角色")]
        public ActionResult Index()
        {
            var vm = CreateViewModel<SystemRoleListViewModel>();
            return PartialView(vm);
        }
        [ControllerOrActionDescription("创建角色")]
        public ActionResult Create()
        {
            var vm = CreateViewModel<SystemRoleViewModel>();
            return PartialView(vm);
        }
        [HttpPost]
        public ActionResult Create(SystemRoleViewModel vm)
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
        [ControllerOrActionDescription("修改角色")]
        public ActionResult Edit(Guid id)
        {
            var vm = CreateViewModel<SystemRoleViewModel>(id);
            return PartialView(vm);
        }
        [HttpPost]
        public ActionResult Edit(SystemRoleViewModel vm)
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
        [ControllerOrActionDescription("删除角色")]
        public ActionResult Delete(Guid[] ids)
        {
            var vm = CreateViewModel<SystemRoleViewModel>(IDs: ids);
            return PartialView(vm);
        }
        [HttpPost]
        public ActionResult Delete(SystemRoleViewModel vm)
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