using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Web.ViewModels.System.SystemUserViewModels
{
    public class SystemUserViewModel : BaseCrudViewModel<SystemUser>
    {
        public override void DoAdd()
        {
            Entity.Password = "000000";
            base.DoAdd();   
        }
    }
}