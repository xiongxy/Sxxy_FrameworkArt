using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sxxy_FrameworkArt.Common.Binders
{
    public class StringBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = base.BindModel(controllerContext, bindingContext);
            if (value is string)
            {
                return (value as string).Replace("\\", "/").Trim();
            }
            return value;
        }
    }
}
