using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;
using Sxxy_FrameworkArt.Models.SystemEntity;

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
        /// <summary>
        /// 初始化页面显示列表
        /// </summary>
        protected override void InitListViewModel()
        {
            List<IGridColumn<SystemUserListView>> listColumns = new List<IGridColumn<SystemUserListView>>();
            listColumns.Add(this.MakeGridColumn(x => x.Id));
            listColumns.Add(this.MakeGridColumn(x => x.CreateTime));
            listColumns.Add(this.MakeGridColumn(x => x.Code));
            listColumns.Add(this.MakeGridColumn(x => x.Name));
            listColumns.Add(this.MakeGridColumn(x => x.Email));
            ListColumns = listColumns;
        }

        public override IOrderedQueryable<SystemUserListView> GetSearchQuery()
        {
            var query = Dc.Set<SystemUser>()
                .Select(x => new SystemUserListView
                {
                    Id = x.Id,
                    Code = x.Code,
                    Email = x.Email,
                    Name = x.Name,
                    CreateTime = x.CreateTime
                })
                .OrderByDescending(x => x.CreateTime);
            var v = query.ToList();
            return query;
        }
    }

    public class SystemUserListView : BaseEntity
    {
        /// <summary>
        /// 编码(用户名)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }

    public class SystemUserSearcher : BaseSearcher
    {
        public string Code { get; set; }
        public string Email { get; set; }
    }
}