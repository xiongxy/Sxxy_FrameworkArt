using System.Web.Mvc;

namespace Sxxy_FrameworkArt.Web.Areas.CoreWebAPI
{
    public class CoreWebAPIAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WebApi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WebApi_default",
                "WebApi/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );
        }
    }
}