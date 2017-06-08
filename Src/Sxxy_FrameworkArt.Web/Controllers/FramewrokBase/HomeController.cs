using System;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Common.Helpers;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    public class HomeController : BaseController
    {
        //[ActionDescription("StartPage")]
        //[AllRights]
        //public ActionResult FrontPage()
        //{
        //    return PartialView();
        //}

        //[ActionDescription("MainPage")]
        //[AllRights]
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[ActionDescription("PopupPage")]
        //[Public]
        //public ActionResult PopUpIndex()
        //{
        //    return View();
        //}

        //[ActionDescription("LeftMenu")]
        //[AllRights]
        //public JsonResult Menu()
        //{
        //    if (IsQuickDebug == true)
        //    {
        //        var tree = Utils.GetAllControllerModules().GetTree();
        //        return Json(tree, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        MenuVM vm = CreateVM<MenuVM>();
        //        var tree = vm.GetMenus().GetTree(x => x.ID.ToString(), x => x.GetMLContentValue(y => y.PageName, null), x => x.Url, x => x.DomainID.ToString());
        //        return Json(tree, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[ActionDescription("TopBar")]
        //[PureHtml]
        //[AllRights]
        //public ActionResult TopBar()
        //{

        //    TopBarVM vm = CreateVM<TopBarVM>();
        //    return PartialView(vm);
        //}

        //[ActionDescription("ChangeLanguage")]
        //[AllRights]
        //public ActionResult ChangeLanguage(string LanguageCode)
        //{
        //    HttpCookie cookie = new HttpCookie(CookiePre + "FFLanguage", LanguageCode);
        //    cookie.Expires = DateTime.Today.AddYears(10);
        //    Response.Cookies.Add(cookie);
        //    return null;
        //}
    }
}