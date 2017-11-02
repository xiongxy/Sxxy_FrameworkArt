using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Web.ViewModels.System.SystemUserViewModels
{
    public class SystemUserViewModel : BaseCrudViewModel<SystemUser>
    {

        public List<SimpleSelectItem> AllRoles { get; set; }
        public List<Guid> SelectedRolsIds { get; set; }
        public SystemUserViewModel()
        {
            SetInclude(x => x.SystemUserAndRoleCorrespondings);
        }

        public override void DoAdd()
        {
            Entity.Password = "000000";
            base.DoAdd();
        }

        protected override void InitVM()
        {
            SelectedRolsIds = Entity.SystemUserAndRoleCorrespondings.Select(x => x.SystemRoleId).ToList();
            AllRoles = Dc.Set<SystemRole>().GetSelectListItems(x => x.RoleName, x => x.Id);
        }
    }
}