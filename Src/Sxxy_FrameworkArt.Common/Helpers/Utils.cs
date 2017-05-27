﻿using System;
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

namespace Sxxy_FrameworkArt.Common.Helpers
{
    public class Utils
    {
        public static List<string> GetAllAccessUrls()
        {
            List<string> rv = new List<string>();
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
    }
}
