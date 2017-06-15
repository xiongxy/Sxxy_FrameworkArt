using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sxxy_FrameworkArt.Common;

namespace Sxxy_FrameworkArt.Web.ViewModels.System.SystemUserViewModels
{
    public class SystemUserListViewModel : BaseListViewModel<SystemUserListView, SystemUserSearcher>
    {
        /// <summary>
        /// 页面显示按钮方案，如果需要增加Action类型则将按钮添加到此类中
        /// </summary>
        public SystemUserListViewModel()
        {
        }

    }

    public class SystemUserListView
    {
    }

    public class SystemUserSearcher
    {
    }
}