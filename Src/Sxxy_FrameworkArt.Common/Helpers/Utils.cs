using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common.Attributes;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Common.Helpers
{
    public class Utils
    {
        public static List<string> GetAllAccessUrls()
        {
            List<string> rv = new List<string>();
            //获取所有程序集
            List<Assembly> allAssemblies = GetAllAssemblies();
            foreach (var item in allAssemblies)
            {
                List<Type> types = new List<Type>();
                types.AddRange(item.GetExportedTypes());
                var reg = types.FirstOrDefault(x => x.IsClass && typeof(IAreaRegistration).IsAssignableFrom(x));
                string area = null;
                if (reg != null)
                {
                    IAreaRegistration obj = reg.GetConstructor(Type.EmptyTypes).Invoke(null) as IAreaRegistration;
                    if (obj != null)
                    {
                        area = obj.AreaRoutePrefix;
                    }
                }
                foreach (var itemType in types)
                {
                    if (itemType.BaseType == typeof(BaseController))
                    {
                        string controllerName = itemType.Name.Replace("Controller", "");
                        bool includeAll = false;
                        object[] attrs = itemType.GetCustomAttributes(typeof(AllRightsAttribute), false);
                        object[] attrs2 = itemType.GetCustomAttributes(typeof(PublicAttribute), false);
                        if (attrs.Length > 0 || attrs2.Length > 0)
                        {
                            includeAll = true;
                        }
                        //获取Contorller下所有方法
                        var methods = itemType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                        methods = methods.Where(x => x.IsSpecialName == false).ToArray();
                        string actionName = "";
                        foreach (var itemMethod in methods)
                        {
                            object[] postAttr = itemMethod.GetCustomAttributes(typeof(HttpPostAttribute), false);
                            if (postAttr.Length == 0)
                            {
                                actionName = itemMethod.Name;
                                string url = controllerName + "/" + actionName;
                                if (area != null)
                                {
                                    url = area + "/" + url;
                                }
                                if (includeAll)
                                {
                                    rv.Add(url);
                                }
                                else
                                {
                                    object[] attrs3 = itemMethod.GetCustomAttributes(typeof(AllRightsAttribute), false);
                                    object[] attrs4 = itemMethod.GetCustomAttributes(typeof(PublicAttribute), false);
                                    if (attrs3.Length > 0 || attrs4.Length > 0)
                                    {
                                        rv.Add(url);
                                    }
                                }
                            }
                        }
                        //再次循环所有方法
                        foreach (var method in methods)
                        {
                            object[] postAttr = method.GetCustomAttributes(typeof(HttpPostAttribute), false);
                            //找到post的方法且没有同名的非post的方法，添加到controller的action列表里
                            if (postAttr.Length > 0 && !rv.Contains(controllerName + "/" + method.Name))
                            {
                                actionName = method.Name;
                                string url = controllerName + "/" + actionName;
                                if (area != null)
                                {
                                    url = area + "/" + url;
                                }
                                if (includeAll == true)
                                {
                                    rv.Add(url);
                                }
                                else
                                {
                                    object[] attrs5 = method.GetCustomAttributes(typeof(AllRightsAttribute), false);
                                    object[] attrs6 = method.GetCustomAttributes(typeof(PublicAttribute), false);
                                    if (attrs5.Length > 0 || attrs6.Length > 0)
                                    {
                                        rv.Add(url);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return rv;
        }

        public static List<Assembly> GetAllAssemblies()
        {
            List<Assembly> allAssemblies = new List<Assembly>();
            string path = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
            //本地调试时dll并不放在一个目录下
            if (path.ToLower().Contains("temporary asp.net files\\root"))
            {
                dir = dir.Parent.Parent;
            }
            foreach (var dll in dir.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                var ass = Assembly.LoadFile(dll.FullName);
                allAssemblies.Add(ass);
            }
            return allAssemblies;
        }

        public static List<SystemModule> GetAllControllerModules()
        {
            List<SystemModule> systemModules = new List<SystemModule>();
            List<Assembly> assemblies = GetAllAssemblies();
            foreach (var assembliesItem in assemblies)
            {
                List<Type> types = new List<Type>();
                try
                {
                    types.AddRange(assembliesItem.GetExportedTypes());
                }
                catch
                {
                    // ignored
                }
                var reg = types.FirstOrDefault(x => x.IsClass && typeof(IAreaRegistration).IsAssignableFrom(x));
                SystemArea systemArea = null;
                if (reg != null)
                {
                    systemArea = new SystemArea();
                    IAreaRegistration obj = reg.GetConstructor(Type.EmptyTypes).Invoke(null) as IAreaRegistration;
                    if (obj != null)
                    {
                        systemArea.Prefix = obj.AreaRoutePrefix;
                        systemArea.AreaName = obj.AreaName;
                    }
                }

                foreach (var item in types)
                {
                    //如果是controller
                    if (item.BaseType == typeof(BaseController))
                    {
                        object[] pubattr = item.GetCustomAttributes(typeof(PublicAttribute), false);
                        object[] arattr = item.GetCustomAttributes(typeof(AllRightsAttribute), false);
                        if (pubattr.Length > 0 || arattr.Length > 0)
                        {
                            continue;
                        }
                        SystemModule model = new SystemModule();
                        model.ClassName = item.Name.Replace("Controller", "");
                        model.NameSpace = item.Namespace;
                        //获取controller上标记的ActionDescription属性的值
                        object[] attrs = item.GetCustomAttributes(typeof(ActionDescriptionAttribute), false);
                        if (attrs.Length > 0)
                        {
                            var ada = attrs[0] as ActionDescriptionAttribute;
                            model.ModuleName = ada.DisplayName;
                        }
                        else
                        {
                            model.ModuleName = model.ClassName;

                        }
                        //获取该controller下所有的方法
                        var methods = item.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                        //过滤掉特殊方法
                        methods = methods.Where(x => x.IsSpecialName == false).ToArray();
                        model.Actions = new List<SystemAction>();
                        //循环所有方法
                        foreach (var method in methods)
                        {
                            object[] pubattr2 = method.GetCustomAttributes(typeof(PublicAttribute), false);
                            object[] arattr2 = method.GetCustomAttributes(typeof(AllRightsAttribute), false);
                            if (pubattr2.Length > 0 || arattr2.Length > 0)
                            {
                                continue;
                            }
                            object[] postAttr = method.GetCustomAttributes(typeof(HttpPostAttribute), false);
                            //如果不是post的方法，则添加到controller的action列表里
                            if (postAttr.Length == 0)
                            {
                                SystemAction action = new SystemAction();
                                action.Module = model;
                                action.MethodName = method.Name;
                                object[] attrs2 = method.GetCustomAttributes(typeof(ActionDescriptionAttribute), false);
                                if (attrs2.Length > 0)
                                {
                                    var ada = attrs2[0] as ActionDescriptionAttribute;
                                    action.ActionName = ada.DisplayName;
                                }
                                else
                                {
                                    action.ActionName = action.MethodName;
                                }
                                var pars = method.GetParameters();
                                if (pars != null && pars.Length > 0)
                                {
                                    action.ParasToRunTest = new List<string>();
                                    foreach (var par in pars)
                                    {
                                        action.ParasToRunTest.Add(par.Name);
                                    }
                                }
                                model.Actions.Add(action);
                            }
                        }
                        //再次循环所有方法
                        foreach (var method in methods)
                        {
                            object[] pubattr2 = method.GetCustomAttributes(typeof(PublicAttribute), false);
                            object[] arattr2 = method.GetCustomAttributes(typeof(AllRightsAttribute), false);
                            if (pubattr2.Length > 0 || arattr2.Length > 0)
                            {
                                continue;
                            }
                            object[] postAttr = method.GetCustomAttributes(typeof(HttpPostAttribute), false);
                            //找到post的方法且没有同名的非post的方法，添加到controller的action列表里
                            if (postAttr.Length > 0 && model.Actions.Where(x => x.MethodName.ToLower() == method.Name.ToLower()).FirstOrDefault() == null)
                            {
                                SystemAction action = new SystemAction();
                                action.Module = model;

                                action.MethodName = method.Name;
                                object[] attrs2 = method.GetCustomAttributes(typeof(ActionDescriptionAttribute), false);
                                if (attrs2.Length > 0)
                                {
                                    var ada = attrs2[0] as ActionDescriptionAttribute;
                                    action.ActionName = ada.DisplayName;

                                }
                                else
                                {
                                    action.ActionName = action.MethodName;

                                }
                                model.Actions.Add(action);
                            }
                        }
                        if (model.Actions != null && model.Actions.Count() > 0)
                        {
                            model.Area = systemArea;
                            systemModules.Add(model);
                        }
                    }
                }
            }
            return systemModules;
        }
    }
}
