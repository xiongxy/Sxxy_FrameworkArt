using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Web.ViewModels.LoginViewModels;

namespace Sxxy_FrameworkArt.Web.Controllers.FramewrokBase
{
    [ControllerOrActionDescription("登录模块")]
    [PublicAttribute]
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
            var vm = CreateViewModel<LoginViewModel>();
            var vvv = Request;
            var vvvv = Request["rd"];
            if (IsQuickDebug)
            {
                vm.Code = "admin";
                vm.Password = "000000";
            }
            return PartialView(vm);
        }
        [HttpPost]
        public ActionResult Index(LoginViewModel vm)
        {
            var user = vm.DoLogin();
            if (user == null)
            {
                return PartialView(vm);
            }
            else
            {
                LoginUserInfo = user;
                string url = "";
                //if (!string.IsNullOrEmpty(vm.Redirect))
                //{
                //    url = vm.Redirect;
                //}
                //else
                //{
                url = "/";

                return Content("<script>window.location.replace('" + HttpUtility.UrlDecode(url) + "');</script>");
            }
            return PartialView(vm);
        }
        [HttpGet]
        public JsonResult ValidateLoginUser(string code, string msg)
        {
            LoginViewModel lv = new LoginViewModel();
            //加密时需要处理此处代码
            string m = lv.LoginUserValidate(code, msg);
            return Json(m, JsonRequestBehavior.AllowGet);
        }
    }
}