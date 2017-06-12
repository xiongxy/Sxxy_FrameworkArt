using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Common.Helpers.Extensions
{
    public static class TreeDataExtension
    {
        /// <summary>
        /// 将一个树形结构Model转成TreeItem类
        /// </summary>
        /// <typeparam name="T">树形结构类</typeparam>
        /// <param name="self">树形结构实例</param>
        /// <param name="key">要付给TreeItem的Key字段的值</param>
        /// <param name="text">要付给TreeItem的Value字段的值</param>
        /// <param name="url">要付给TreeItem的Url字段的值</param>
        /// <param name="tag">要付给TreeItem的Tag字段的值</param>
        /// <returns>转换后的TreeItem类</returns>
        public static TreeItem GetTreeItem<T>(this T self, Expression<Func<T, string>> key
            , Expression<Func<T, string>> text
            , Expression<Func<T, string>> url = null
            , Expression<Func<T, string>> domainid = null
            , Expression<Func<T, string>> tag = null)
            where T : BaseEntity, ITreeData<T>
        {
            //给TreeItem赋值
            TreeItem v = new TreeItem();
            v.Key = key == null ? "" : key.Compile().Invoke(self);
            v.Title = text == null ? "" : text.Compile().Invoke(self);
            v.Url = url == null ? "" : url.Compile().Invoke(self);
            v.Tag = tag == null ? "" : tag.Compile().Invoke(self);
            v.Domainid = domainid == null ? "" : domainid.Compile().Invoke(self);
            var children = self.GetChildren();
            if (children != null && children.Count() > 0)
            {
                v.IsFolder = true;
                v.ChildrensItems = new List<TreeItem>();
                foreach (var child in children)
                {
                    v.ChildrensItems.Add(child.GetTreeItem(key, text, url, domainid, tag));
                }
            }
            return v;
        }
        public static List<TreeItem> GetTree<T>(this IEnumerable<T> self
            , Expression<Func<T, string>> key
            , Expression<Func<T, string>> text
            , Expression<Func<T, string>> url = null
            , Expression<Func<T, string>> domainid = null
            , Expression<Func<T, string>> tag = null)
            where T : BaseEntity, ITreeData<T>
        {
            List<TreeItem> v = new List<TreeItem>();
            foreach (var item in self)
            {
                v.Add(item.GetTreeItem(key, text, url, domainid, tag));
            }
            return v;
        }
        public static List<TreeItem> GetTree<T>(this IEnumerable<T> self)
            where T : SystemModule
        {
            List<TreeItem> rv = new List<TreeItem>();
            //排除Error，Home和Login的Controller，因为他们不需要出现在任何树形列表中
            //根据其他Controller的命名空间分组
            var group = self.Where(x => x.ClassName != "Error" && x.ClassName != "Home" && x.ClassName != "Login").GroupBy(x => x.NameSpace, x => x);
            int si = 0;
            //将命名空间作为树形结构最顶级的项目
            foreach (var g in group)
            {
                TreeItem mti = new TreeItem();
                mti.Key = si.ToString();
                string ns = g.Key;
                ns = ns.Substring(ns.LastIndexOf('.') + 1);
                mti.Title = ns;
                mti.Url = null;
                mti.IsFolder = true;
                mti.ChildrensItems = new List<TreeItem>();
                //循环命名空间下的Controller，将其作为第二级项目
                var items = g.ToList();
                int mi = 0;
                foreach (var item in items)
                {
                    TreeItem ti = new TreeItem();
                    ti.Key = mi.ToString();
                    ti.Title = item.ModuleName;
                    ti.Url = null;
                    ti.IsFolder = true;
                    ti.ChildrensItems = new List<TreeItem>();
                    int ai = 0;
                    //循环Controller方法，作为最底层的项目
                    foreach (var chi in item.Actions)
                    {
                        TreeItem sti = new TreeItem();
                        sti.Key = mi + "_" + ai;
                        sti.Title = chi.ActionName;
                        if (item.Area == null)
                        {
                            sti.Url = "/" + item.ClassName + "/" + chi.MethodName;
                        }
                        else
                        {
                            sti.Url = "/" + item.Area.Prefix + "/" + item.ClassName + "/" + chi.MethodName;
                        }
                        //将ParasToRunTest保存在tag中
                        if (chi.ParasToRunTest != null && chi.ParasToRunTest.Count > 0)
                        {
                            string tag = "";
                            for (int i = 0; i < chi.ParasToRunTest.Count; i++)
                            {
                                tag += chi.ParasToRunTest[i];
                                if (i < chi.ParasToRunTest.Count - 1)
                                {
                                    tag += ";";
                                }
                            }
                            sti.Tag = tag;
                        }
                        ti.ChildrensItems.Add(sti);
                        ai++;
                    }
                    mti.ChildrensItems.Add(ti);
                    mi++;
                }
                rv.Add(mti);
                si++;
            }

            return rv;
        }
    }
}
