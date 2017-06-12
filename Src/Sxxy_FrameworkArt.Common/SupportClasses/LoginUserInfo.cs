using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Common.SupportClasses
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class LoginUserInfo
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 用户的角色
        /// </summary>
        public List<SystemRole> Roles { get; set; }
        /// <summary>
        /// 用户的页面权限列表
        /// </summary>
        public List<FunctionPrivilege> FunctionPrivileges { get; set; }
        /// <summary>
        /// 用户的数据权限列表
        /// </summary>
        public List<DataPrivilege> DataPrivileges { get; set; }

        /// <summary>
        /// 判断URL 是否有权限访问
        /// </summary>
        /// <param name="url"></param>
        /// <returns>true 表示可以访问，false 表示不可以访问</returns>
        public bool IsAccess(string url)
        {
            if (string.IsNullOrEmpty(url))
                return true;
            if (BaseController.IsQuickDebug)
                return true;
            //循环所有不限制访问的url，如果含有当前判断的url，则认为可以访问
            foreach (var au in BaseController.AllRightActions)
            {
                Regex r = new Regex(au + "[/\\?]?");
                if (r.IsMatch(url))
                {
                    return true;
                }
            }
            //如果没有页面权限，则直接返回false
            if (FunctionPrivileges == null)
            {
                return false;
            }
            //如果url以#开头，一般是javascript使用的临时地址，不需要判断，直接返回true
            url = url.Trim();
            if (url.StartsWith("#"))
            {
                return true;
            }
            //获取系统菜单
            List<SystemMenu> menus = BaseController.SystemMenuProperty;
            //寻找菜单中是否有与当前判断的url完全相同的
            var menu = menus.FirstOrDefault(x => x.Url != null && x.Url.ToLower() == url.ToLower());

            //如果没有，抹掉当前url的参数，用不带参数的url比对
            if (menu == null)
            {
                int pos = url.IndexOf("?");
                if (pos > 0)
                {
                    url = url.Substring(0, pos);
                    menu = menus.Where(x => x.ActionId != null && x.Url != null && x.Url.ToLower() == url.ToLower()).FirstOrDefault();
                }
            }
            //如果还没找到，则判断url是否为/controller/action/id这种格式，如果是则抹掉/id之后再对比
            if (menu == null)
            {
                var split = url.Split('/');
                long longTest;
                if (split.Length >= 2 && long.TryParse(split.Last(), out longTest))
                {
                    int pos = url.LastIndexOf("/");
                    url = url.Substring(0, pos);
                    menu = menus.Where(x => x.ActionId != null && x.Url != null && x.Url.ToLower() == url.ToLower()).FirstOrDefault();
                }
            }
            //如果最终没有找到，说明系统菜单中并没有配置这个url，返回false
            if (menu == null)
            {
                return false;
            }
            //如果找到了，则继续验证其他权限
            else
            {
                return IsAccess(menu, menus);
            }

        }

        private bool IsAccess(SystemMenu menu, List<SystemMenu> menus)
        {
            //如果当前是QuickDebug模式，则所有url都可以访问
            if (BaseController.IsQuickDebug == true)
            {
                return true;
            }
            //寻找当前菜单的页面权限
            var find = FunctionPrivileges.Where(x => x.MenuItemId == menu.Id).ToList();
            //如果能找到直接对应的页面权限
            if (find.Count > 0)
            {
                //检查是否有拒绝访问的设定，如果有则直接返回false
                var deny = find.Where(x => x.Allowed == false && x.UserId != null).FirstOrDefault();
                if (deny != null)
                {
                    return false;
                }
                else
                {
                    //检查是否有允许访问的设定，如果有则直接返回true
                    var allow = find.Where(x => x.Allowed == true && x.UserId != null).FirstOrDefault();
                    if (allow != null)
                    {
                        return true;
                    }
                    else
                    {
                        //如果用户没有指定任何页面权限，则检查用户所属角色中是否有拒绝访问的设定，如果有则返回false
                        var roleDeny = find.Where(x => x.Allowed == false && x.RoleId != null).FirstOrDefault();
                        if (roleDeny != null)
                        {
                            return false;
                        }
                        //如果没有则返回true，因为find里面有值，但前三种情况都没有，则肯定是允许角色访问是有的，所以不再做判断了
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            //如果没有直接对应的，且如果当前menu设定了继承属性，则递归寻找上层菜单是否可以访问
            else
            {
                if (menu.Parent == null || menu.IsInherit == false)
                {
                    return false;
                }
                else
                {
                    return IsAccess(menu.Parent, menus);
                }
            }
        }
    }
}
