using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;
using Sxxy_FrameworkArt.Models.SystemEntity;
using Sxxy_FrameworkArt.Common.SupportClasses;

namespace Sxxy_FrameworkArt.Web.ViewModels.SharedViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public List<SystemMenu> GetMenus(LoginUserInfo info = null)
        {
            //选择在菜单上显示的顶级菜单
            var temp = SystemMenuProperty
                .Where(x =>
                    x.ShowOnMenu == true &&
                    x.Parent == null)
                .ToList();
            //将FFMenus深度复制到局部变量Menus上。因为FFMenus是全剧静态变量，而我们需要根据当前用户的权限过滤掉一部分菜单，如果直接操作FFMenus则会导致FFMenus的数据永久变化
            var Menus = new List<SystemMenu>();
            foreach (var item in temp)
            {
                DeepCloneMenu(Menus, item, null);
            }
            //移除没有权限访问的菜单
            RemoveUnAccessableMenu(Menus, info);
            //移除隐藏的菜单
            RemoveHiddenMenu(Menus);
            //移除空菜单目录
            RemoveEmptyMenu(Menus);
            return Menus;
        }
        private void DeepCloneMenu(List<SystemMenu> menus, SystemMenu item, SystemMenu o)
        {
            SystemMenu v = new SystemMenu();
            v.Id = item.Id;
            v.ActionId = item.ActionId;
            v.ModuleId = item.ModuleId;
            v.DisplayOrder = item.DisplayOrder;
            v.FolderOnly = item.FolderOnly;
            v.IsPublic = item.IsPublic;
            v.ShowOnMenu = item.ShowOnMenu;
            v.ParentId = item.ParentId;
            v.Url = item.Url;
            v.PageName = item.PageName;
            //递归调用本函数，复制子节点
            v.Children = new List<SystemMenu>();
            if (item.Children != null)
            {
                foreach (var child in item.Children)
                {
                    DeepCloneMenu(menus, child, v);
                }
            }
            //将新建的菜单添加到集合
            if (o == null)
            {
                menus.Add(v);
            }
            else
            {
                o.Children.Add(v);
            }
        }
        private void RemoveHiddenMenu(List<SystemMenu> menus)
        {
            if (menus == null)
            {
                return;
            }
            //循环菜单集合，将所有ShowOnMenu属性为false的移除
            List<SystemMenu> toRemove = new List<SystemMenu>();
            foreach (var menu in menus)
            {
                if (menu.ShowOnMenu == false)
                {
                    toRemove.Add(menu);
                }
                else
                {
                    RemoveHiddenMenu(menu.Children);
                }
            }
            foreach (var remove in toRemove)
            {
                menus.Remove(remove);
            }
        }
        /// <summary>
        /// 移除空菜单目录
        /// </summary>
        /// <param name="menus">菜单集合</param>
        private void RemoveEmptyMenu(List<SystemMenu> menus)
        {
            List<SystemMenu> ToRemove = new List<SystemMenu>();
            //循环菜单集合
            foreach (var item in menus)
            {
                ////如果菜单是目录，且目录下没有子节点，则移除这个空目录
                //if (item.FolderOnly == true)
                //{
                //    var pages = item.GetAllChildren();
                //    if (pages == null || pages.All(x => x.FolderOnly == true))
                //    {
                //        ToRemove.Add(item);
                //    }
                //}
                //如果有子节点，则递归调用本函数，处理子节点
                if (item.Children != null && item.Children.Count > 0)
                {
                    RemoveEmptyMenu(item.Children);
                }
            }
            //移除标记的菜单
            foreach (var item in ToRemove)
            {
                menus.Remove(item);
            }
        }
        /// <summary>
        /// 移除没有权限访问的菜单
        /// </summary>
        /// <param name="menus">菜单列表</param>
        /// <param name="info">用户信息</param>
        private void RemoveUnAccessableMenu(List<SystemMenu> menus, LoginUserInfo info)
        {
            if (menus == null)
            {
                return;
            }

            List<SystemMenu> toRemove = new List<SystemMenu>();
            //如果没有指定用户信息，则用当前用户的登录信息
            if (info == null)
            {
                info = LoginUserInfo;
            }
            //循环所有菜单项
            foreach (var menu in menus)
            {
                ////判断是否有权限，如果没有，则添加到需要移除的列表中
                //if (!string.IsNullOrEmpty(menu.Url) && info.IsAccessable(menu.Url) == false)
                //{
                //    toRemove.Add(menu);
                //}
                ////如果有权限，则递归调用本函数检查子菜单
                //else
                //{
                //    RemoveUnAccessableMenu(menu.Children, info);
                //}
            }
            //删除没有权限访问的菜单
            foreach (var remove in toRemove)
            {
                menus.Remove(remove);
            }
        }
        public string RutenMenuHtml()
        {
            List<TreeItem> tree;
            if (BaseController.IsQuickDebug)
            {
                tree = Utils.GetAllControllerModules().GetTree();
            }
            else
            {
                tree = GetMenus().GetTree(x => x.Id.ToString(), x => x.PageName, x => x.Url);
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in tree)
            {
                sb.Append($"<li class=\"header\">{item.Title}</li>");
                foreach (var childrensItem in item.ChildrensItems)
                {
                    if (childrensItem.ChildrensItems != null)
                    {
                        sb.Append("<li class=\"treeview\">");
                        sb.Append("<a href=\"#\">");
                    }
                    else
                    {
                        sb.Append("<li>");
                        sb.Append($"<a href=\"/#{childrensItem.Url}\">");
                    }
                    sb.Append("<i class=\"fa fa-files-o\"></i>");
                    sb.Append($"<span>{childrensItem.Title}</span>");
                    // 提示容器
                    //  <span class="pull-right-container">
                    //    <span class="label label-primary pull-right">4</span> //表示数字
                    //    <small class="label pull-right bg-green">new</small>  //表示字母
                    //  </span>
                    sb.Append("</a>");
                    if (childrensItem.ChildrensItems != null)
                    {
                        sb.Append("<ul class=\"treeview-menu\">");
                        foreach (var childrensItemChildrensItem in childrensItem.ChildrensItems)
                        {
                            sb.Append(
                                $"<li><a href=\"/#{childrensItemChildrensItem.Url}\"><i class=\"fa fa-circle-o\"></i>{childrensItemChildrensItem.Title}</a></li>");
                        }
                        sb.Append("</ul>");
                    }
                    sb.Append("</li>");
                }
            }
            return sb.ToString();
        }
    }
}