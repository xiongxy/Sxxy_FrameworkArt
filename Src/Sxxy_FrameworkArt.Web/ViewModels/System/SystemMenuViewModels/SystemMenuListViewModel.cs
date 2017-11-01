using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;
using Sxxy_FrameworkArt.Models.SystemEntity;
using Sxxy_FrameworkArt.Web.ViewModels.System.SystemUserViewModels;

namespace Sxxy_FrameworkArt.Web.ViewModels.System.SystemMenuViewModels
{
    public class SystemMenuListViewModel : BaseListViewModel<SystemMenuListView, SystemMenuSearcher>
    {
        public SystemMenuListViewModel()
        {
            GridActions = new List<GridAction>();
            GridActions.Add(GridAction.MakeStandAction("SystemMenu", GridActionStandardTypesEnum.Create));
            GridActions.Add(GridAction.MakeStandAction("SystemMenu", GridActionStandardTypesEnum.Edit));
            GridActions.Add(GridAction.MakeStandAction("SystemMenu", GridActionStandardTypesEnum.Delete));
            GridActions.Add(GridAction.MakeNormalAction("SystemMenu", "RefreshMenu","刷新菜单"));
        }
        protected override void InitListViewModel()
        {
            List<IGridColumn<SystemMenuListView>> listColumns = new List<IGridColumn<SystemMenuListView>>();
            listColumns.Add(this.MakeGridColumn(x => x.Id));
            listColumns.Add(this.MakeGridColumn(x => x.PageName));
            listColumns.Add(this.MakeGridColumn(x => x.ActionName));
            listColumns.Add(this.MakeGridColumn(x => x.ModuleName));
            listColumns.Add(this.MakeGridColumn(x => x.FolderOnly));
            listColumns.Add(this.MakeGridColumn(x => x.IsInherit));
            listColumns.Add(this.MakeGridColumn(x => x.ParentId, false));
            listColumns.Add(this.MakeGridColumn(x => x.CreateTime));
            ListColumns = listColumns;
        }
        public override IOrderedQueryable<SystemMenuListView> GetSearchQuery()
        {
            var query = Dc.Set<SystemMenu>()
                .Select(x => new SystemMenuListView
                {
                    Id = x.Id,
                    PageName = x.PageName,
                    ActionName = x.ActionName,
                    ModuleName = x.ModuleName,
                    FolderOnly = x.FolderOnly,
                    ParentId = x.ParentId,
                    CreateTime = x.CreateTime
                })
                .OrderBy(x => x.CreateTime);
            var v = query.ToList();
            return query;
        }
    }
    public class SystemMenuListView : BaseEntity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 是否是文件夹
        /// </summary>
        public bool? FolderOnly { get; set; }
        /// <summary>
        /// 是否继承
        /// </summary>
        public bool? IsInherit { get; set; }

        public Guid? ParentId { get; set; }
    }
    public class SystemMenuSearcher : BaseSearcher
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
    }
}