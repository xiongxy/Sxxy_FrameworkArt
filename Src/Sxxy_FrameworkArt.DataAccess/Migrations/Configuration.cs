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
            SystemRole systemRole = new SystemRole
            {
                Id = Guid.NewGuid(),
                RoleCode = "000001",
                CreateTime = DateTime.Now,
                RoleName = "管理员"
            };
            dataContext.Set<SystemRole>().Add(systemRole);
            dataContext.SaveChanges();

            #endregion
            #region 用户初始化
            dataContext = dataContext.ReCreate();
            dataContext.Set<SystemRole>().Attach(systemRole);
            SystemUser systemUser = new SystemUser
            {
                Id = Guid.NewGuid(),
                Code = "admin",
                Password = "000000",
                StartWorkDate = DateTime.Now,
                Name = "Admin",
                CreateTime = DateTime.Now,
                IsValid = true
            };
            dataContext.Set<SystemUser>().Add(systemUser);
            dataContext.SaveChanges();
            #endregion
            #region 用户角色对应关系初始化
            dataContext = dataContext.ReCreate();
            SystemUserAndRoleCorresponding systemUserAndRoleCorresponding = new SystemUserAndRoleCorresponding()
            {
                SystemRoleId = systemRole.Id,
                SystemUserId = systemUser.Id
            };
            dataContext.Set<SystemUserAndRoleCorresponding>().Add(systemUserAndRoleCorresponding);
            dataContext.SaveChanges();
            #endregion
            #region 菜单初始化
            dataContext = dataContext.ReCreate();
            dataContext.Set<SystemRole>().Attach(systemRole);
            #region 系统管理
            SystemMenu systemManagement = GetFolderMenu("系统管理");
            //SystemMenu logList = GetMenu(allModules, "Admin", "ActionLog", "Index", new List<SystemRole> { adminRole }, null, 1);
            //SystemMenu companyList = GetMenu(allModules, null, "FrameworkCompany", "Index", new List<SystemRole> { adminRole }, null, 2);
            //SystemMenu departmentList = GetMenu(allModules, null, "FrameworkDepartment", "Index", new List<SystemRole> { adminRole }, null, 3);
            //SystemMenu roleList = GetMenu(allModules, "Admin", "FrameworkRole", "Index", new List<SystemRole> { adminRole }, null, 4);
            SystemMenu roleList = GetMenu(allModules, "SystemRole", "Index", 4);
            SystemMenu userList = GetMenu(allModules, "SystemUser", "Index", 5);
            SystemMenu menuList = GetMenu(allModules, "SystemMenu", "Index", 6);
            ////添加HomeController下的通用方法，主要是一些导出的通用函数
            var exportActs = allModules.Where(x => x.ClassName == "Home").SelectMany(x => x.Actions).ToList();
            foreach (var exp in exportActs)
            {
                dataContext.Set<SystemMenu>().Add(GetMenuFromAction(exp, false));
            }
            systemManagement.Children.AddRange(new SystemMenu[] { menuList, userList, roleList });
            #endregion
            dataContext.Set<SystemMenu>().Add(systemManagement);
            dataContext.SaveChanges();
            #endregion
        }
        private SystemMenu GetMenu(List<SystemModule> systemModules, string controllerName, string actionName, int displayOrder = 1)
        {
            var acts = systemModules.Where(x => x.ClassName == controllerName).SelectMany(x => x.Actions).ToList();
            var act = acts.SingleOrDefault(x => x.MethodName == actionName);
            var rest = acts.Where(x => x.MethodName != actionName).ToList();
            SystemMenu menu = GetMenuFromAction(act, true, displayOrder);
            for (int i = 0; i < rest.Count; i++)
            {
                menu.Children.Add(GetMenuFromAction(rest[i], false, (i + 1)));
            }
            return menu;
        }
        private SystemMenu GetFolderMenu(string folderText, bool isShowOnMenu = true)
        {
            SystemMenu menu = new SystemMenu
            {
                Id = Guid.NewGuid(),
                PageName = folderText,
                Children = new List<SystemMenu>(),
                ShowOnMenu = isShowOnMenu,
                FolderOnly = true,
                IsPublic = false,
                CreateTime = DateTime.Now,
                DisplayOrder = 1
            };
            return menu;
        }
        private SystemMenu GetMenuFromAction(SystemAction systemAction, bool isShowOnMenu, int displayOrder = 1)
        {
            SystemMenu systemMenu = new SystemMenu
            {
                Id = Guid.NewGuid(),
                ActionId = systemAction.Id,
                ModuleId = systemAction.ModuleId,
                Url = "/" + systemAction.Module.ClassName + "/" + systemAction.MethodName,
                ShowOnMenu = isShowOnMenu,
                FolderOnly = false,
                Children = new List<SystemMenu>(),
                IsPublic = false,
                DisplayOrder = displayOrder,
                CreateTime = DateTime.Now
            };
            if (isShowOnMenu)
            {
                systemMenu.PageName = systemAction.Module.ModuleName;
                systemMenu.ActionName = systemAction.ActionName;
            }
            return systemMenu;
        }
    }
}
