using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.SupportClasses
{
    /// <summary>
    /// 列表动作类，负责处理列表动作条中的动作按钮
    /// </summary>
    public class GridAction
    {
        #region 属性  Property
        /// <summary>
        /// 按钮Id
        /// </summary>
        public string ButtonId { get; set; }
        /// <summary>
        /// 按钮名称（动作名称）
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 按钮的图标样式
        /// </summary>
        public string IconCls { get; set; }
        /// <summary>
        /// 动作的Area
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 动作的Action 
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 动作的Controller
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 动作的Url（根据Area Action Controller 生成）
        /// </summary>
        public string Url
        {
            get
            {
                string url = "/" + this.ControllerName + "/" + this.ActionName;
                if (!string.IsNullOrEmpty(this.Area))
                {
                    url = "/" + this.Area + url;
                }
                return url;
            }
        }
        #endregion

        /// <summary>
        /// 创建标准动作
        /// </summary>
        /// <param name="controllerName">控制器名称(Controller后缀不写)</param>
        /// <param name="actionType">动作类型</param>
        /// <param name="name">按钮名称(默认)</param>
        /// <param name="buttonId">按钮Id(默认)</param>
        /// <returns></returns>
        public static GridAction MakeStandAction(string controllerName, GridActionStandardTypesEnum actionType, string name = null, string buttonId = null)
        {
            string iconCls = "";
            string actionName = "";
            string gridName = "";
            switch (actionType)
            {
                case GridActionStandardTypesEnum.Create:
                    iconCls = "glyphicon glyphicon-plus";
                    actionName = "Create";
                    gridName = "创建";
                    break;
                case GridActionStandardTypesEnum.Edit:
                    iconCls = "glyphicon glyphicon-edit";
                    actionName = "Edit";
                    gridName = "修改";
                    break;
                case GridActionStandardTypesEnum.Delete:
                    iconCls = "glyphicon glyphicon-trash";
                    actionName = "Delete";
                    gridName = "删除";
                    break;
            }
            return new GridAction() { ButtonId = "", Name = name ?? gridName, ActionName = actionName, ControllerName = controllerName, IconCls = iconCls };
        }
    }

    public enum GridActionStandardTypesEnum
    {
        Create = 1,
        Edit = 2,
        Delete = 3
    }
}