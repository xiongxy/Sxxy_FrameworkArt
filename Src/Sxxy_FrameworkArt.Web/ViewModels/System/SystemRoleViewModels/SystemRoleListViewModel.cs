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

namespace Sxxy_FrameworkArt.Web.ViewModels.System.SystemRoleViewModels
{
    public class SystemRoleListViewModel : BaseListViewModel<SystemRoleListView, SystemRoleSearcher>
    {
        /// <inheritdoc />
        /// <summary>
        /// 页面显示按钮方案，如果需要增加Action类型则将按钮添加到此类中
        /// </summary>
        public SystemRoleListViewModel()
        {
            GridActions = new List<GridAction>();
            GridActions.Add(GridAction.MakeStandAction("SystemRole", GridActionStandardTypesEnum.Create));
            GridActions.Add(GridAction.MakeStandAction("SystemRole", GridActionStandardTypesEnum.Edit));
            GridActions.Add(GridAction.MakeStandAction("SystemRole", GridActionStandardTypesEnum.Delete));
        }
        /// <summary>
        /// 初始化页面显示列表
        /// </summary>
        protected override void InitListViewModel()
        {
            //选择要显示的列
            List<IGridColumn<SystemRoleListView>> listColumns = new List<IGridColumn<SystemRoleListView>>();
            listColumns.Add(this.MakeGridColumn(x => x.Id));
            listColumns.Add(this.MakeGridColumn(x => x.RoleCode));
            listColumns.Add(this.MakeGridColumn(x => x.RoleName));
            listColumns.Add(this.MakeGridColumn(x => x.RoleRemark));
            listColumns.Add(this.MakeGridColumn(x => x.CreateTime));
            listColumns.Add(this.MakeGridActionColumn());
            ListColumns = listColumns;

        }
        /// <summary>
        /// 查询计划
        /// </summary>
        /// <returns></returns>
        public override IOrderedQueryable<SystemRoleListView> GetSearchQuery()
        {
            var query = Dc.Set<SystemRole>()
                .Where(x =>
                (string.IsNullOrEmpty(Searcher.RoleCode) || x.RoleCode.Contains(Searcher.RoleCode)) &&
                (string.IsNullOrEmpty(Searcher.RoleName) || x.RoleName.Contains(Searcher.RoleName)))
                .Select(x => new SystemRoleListView
                {
                    Id = x.Id,
                    RoleCode = x.RoleCode,
                    RoleName = x.RoleName,
                    RoleRemark = x.RoleRemark,
                    CreateTime = x.CreateTime
                })
                .OrderBy(x => x.CreateTime);
            return query;
        }
    }
    public class SystemRoleListView : BaseEntity
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色简介
        /// </summary>
        public string RoleRemark { get; set; }
    }
    public class SystemRoleSearcher : BaseSearcher
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}