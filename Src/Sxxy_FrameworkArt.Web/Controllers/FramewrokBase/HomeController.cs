﻿using System;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;
using Sxxy_FrameworkArt.Web.ViewModels.SharedViewModels;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    [ControllerOrActionDescription("主模块")]
    public class HomeController : BaseController
    {
        [ControllerOrActionDescription("初始页")]
        public ActionResult Index()
        {
            var vm = CreateViewModel<LayoutViewModel>();
            return View(vm);
        }
        [HttpGet]
        public ActionResult LeftMenu()
        {
            var vm = CreateViewModel<MenuViewModel>();
            var str = vm.RutenMenuHtml();
            return Content(str);
        }
        [HttpGet]
        public ActionResult RouteGuidance()
        {
            var vm = CreateViewModel<RouteGuidanceViewModel>();
            //var str = vm.RutenMenuHtml();
            return Content("");
        }
    }
}