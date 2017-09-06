using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.DataAccess.Migrations
{
    public class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            // 默认情况下不会自动迁移数据库结构，还有许多其它相关设置，详看 DbMigrationsConfiguration<TContext>
            AutomaticMigrationsEnabled = true;
            //获取或设置 指示是否可接受自动迁移期间的数据丢失的值。如果设置为false，则将在数据丢失可能作为自动迁移一部分出现时引发异常。
            AutomaticMigrationDataLossAllowed = true;
        }
        protected override void Seed(DataContext context)
        {


            IDataContext dataContext = context as IDataContext;
            dataContext = dataContext.ReCreate();
            if (dataContext.Set<SystemUser>().Any())
            {
                return;
            }
            //try
            //{
            #region  模块初始化
            dataContext = dataContext.ReCreate();
            var allModules = Utils.GetAllControllerModules();
            foreach (var item in allModules)
            {
                item.Id = Guid.NewGuid();
                item.CreateTime = DateTime.Now;
                dataContext.Set<SystemModule>().Add(item);
            }
            dataContext.SaveChanges();
            #endregion
            #region 角色初始化

            dataContext = dataContext.ReCreate();
            SystemRole adminRole = new SystemRole
            {
                Id = Guid.NewGuid(),
                RoleCode = "000001",
                CreateTime = DateTime.Now,
                RoleName = "管理员"
            };
            dataContext.Set<SystemRole>().Add(adminRole);
            dataContext.SaveChanges();

            #endregion
            #region 用户初始化
            dataContext = dataContext.ReCreate();
            dataContext.Set<SystemRole>().Attach(adminRole);
            SystemUser adminUser = new SystemUser
            {
                Id = Guid.NewGuid(),
                Code = "admin",
                Password = "000000",
                StartWorkDate = DateTime.Now,
                Roles = new List<SystemRole>(new SystemRole[] { adminRole }),
                Name = "Admin",
                CreateTime = DateTime.Now,
                IsValid = true
            };
            dataContext.Set<SystemUser>().Add(adminUser);
            dataContext.SaveChanges();
            #endregion
            #region 菜单初始化
            dataContext = dataContext.ReCreate();
            dataContext.Set<SystemRole>().Attach(adminRole);
            #region 系统管理

            SystemMenu systemManagement = GetFolderMenu("系统管理", new List<SystemRole> { adminRole }, null);
            //SystemMenu logList = GetMenu(allModules, "Admin", "ActionLog", "Index", new List<SystemRole> { adminRole }, null, 1);
            //SystemMenu companyList = GetMenu(allModules, null, "FrameworkCompany", "Index", new List<SystemRole> { adminRole }, null, 2);
            //SystemMenu departmentList = GetMenu(allModules, null, "FrameworkDepartment", "Index", new List<SystemRole> { adminRole }, null, 3);
            //SystemMenu roleList = GetMenu(allModules, "Admin", "FrameworkRole", "Index", new List<SystemRole> { adminRole }, null, 4);
            SystemMenu userList = GetMenu(allModules, null, "SystemUser", "Index", new List<SystemRole> { adminRole }, null, 5);
            SystemMenu menuList = GetMenu(allModules, null, "SystemMenu", "Index", new List<SystemRole> { adminRole }, null, 6);
            //SystemMenu dpList = GetMenu(allModules, "Admin", "DataPrivilege", "Index", new List<SystemRole> { adminRole }, null, 7);
            //SystemMenu domainList = GetMenu(allModules, "Admin", "FrameworkDomain", "Index", new List<SystemRole> { adminRole }, null, 8);
            ////添加HomeController下的通用方法，主要是一些导出的通用函数
            var ExportActs = allModules.Where(x => x.ClassName == "Home" && x.Area != null && x.Area.Prefix.ToLower() == "webapi").SelectMany(x => x.Actions).ToList();
            foreach (var exp in ExportActs)
            {
                dataContext.Set<SystemMenu>().Add(GetMenuFromAction(exp, false, new List<SystemRole> { adminRole }, null));
            }
            //systemManagement.Children.AddRange(new SystemMenu[] { logList, companyList, departmentList, roleList, userList, menuList, dpList, domainList });
            systemManagement.Children.AddRange(new SystemMenu[] { menuList, userList });
            #endregion

            dataContext.Set<SystemMenu>().Add(systemManagement);
            dataContext.SaveChanges();

            #endregion
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
        }
        private SystemMenu GetMenu(List<SystemModule> allModules, string areaName, string controllerName, string actionName, List<SystemRole> allowedRoles, List<SystemUser> allowedUsers, int displayOrder)
        {
            var acts = allModules.Where(x => x.ClassName == controllerName && (areaName == null || x.Area.Prefix.ToLower() == areaName.ToLower())).SelectMany(x => x.Actions).ToList();
            var act = acts.Where(x => x.MethodName == actionName).SingleOrDefault();
            var rest = acts.Where(x => x.MethodName != actionName).ToList();
            SystemMenu menu = GetMenuFromAction(act, true, allowedRoles, allowedUsers, displayOrder);
            for (int i = 0; i < rest.Count; i++)
            {
                menu.Children.Add(GetMenuFromAction(rest[i], false, allowedRoles, allowedUsers, (i + 1)));
            }
            return menu;
        }
        private SystemMenu GetFolderMenu(string FolderText, List<SystemRole> allowedRoles, List<SystemUser> allowedUsers, bool isShowOnMenu = true, bool isInherite = false)
        {
            SystemMenu menu = new SystemMenu
            {
                Id = Guid.NewGuid(),
                PageName = FolderText,
                Children = new List<SystemMenu>(),
                //Privileges = new List<FunctionPrivilege>(),
                ShowOnMenu = isShowOnMenu,
                IsInherit = isInherite,
                //IsInside = true,
                FolderOnly = true,
                IsPublic = false,
                CreateTime = DateTime.Now,
                DisplayOrder = 1
            };
            if (allowedRoles != null)
            {
                foreach (var role in allowedRoles)
                {
                    // menu.Privileges.Add(new FunctionPrivilege { RoleId = role.Id, Allowed = true });
                }
            }
            if (allowedUsers != null)
            {
                foreach (var user in allowedUsers)
                {
                    //menu.Privileges.Add(new FunctionPrivilege { UserId = user.Id, Allowed = true });
                }
            }

            return menu;
        }
        private SystemMenu GetMenuFromAction(SystemAction act, bool isMainLink, List<SystemRole> allowedRoles, List<SystemUser> allowedUsers, int displayOrder = 1)
        {
            SystemMenu menu = new SystemMenu
            {
                Id = Guid.NewGuid(),
                ActionId = act.Id,
                ModuleId = act.ModuleId,
                Url = "/" + act.Module.ClassName + "/" + act.MethodName,
                //Privileges = new List<FunctionPrivilege>(),
                ShowOnMenu = isMainLink,
                FolderOnly = false,
                Children = new List<SystemMenu>(),
                IsInherit = true,
                IsPublic = false,
                //IsInside = true,
                DisplayOrder = displayOrder,
                CreateTime = DateTime.Now
            };
            if (act.Module.Area != null)
            {
                menu.Url = "/" + act.Module.Area.Prefix + menu.Url;
            }
            if (isMainLink)
            {
                menu.PageName = act.Module.ModuleName;
                menu.ModuleName = act.Module.ModuleName;
                menu.ActionName = act.ActionName;
            }
            else
            {
                menu.PageName = act.ActionName;
                menu.ActionName = act.ActionName;
                menu.ModuleName = act.Module.ModuleName;
            }
            if (allowedRoles != null)
            {
                foreach (var role in allowedRoles)
                {
                    //menu.Privileges.Add(new FunctionPrivilege { RoleId = role.Id, Allowed = true });

                }
            }
            if (allowedUsers != null)
            {
                foreach (var user in allowedUsers)
                {
                    //menu.Privileges.Add(new FunctionPrivilege { UserId = user.Id, Allowed = true });
                }
            }
            return menu;
        }
    }
}
