using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Common.Helpers
{
    public class PropertyHelper
    {
        public static string GetPropertyName(Expression expression, bool getAll = true)
        {
            if (expression == null)
            {
                return "";
            }
            MemberExpression me = null;
            LambdaExpression le = null;
            if (expression is MemberExpression)
            {
                me = expression as MemberExpression;
            }
            if (expression is LambdaExpression)
            {
                le = expression as LambdaExpression;
                if (le.Body is MemberExpression)
                {
                    me = le.Body as MemberExpression;
                }
                if (le.Body is UnaryExpression)
                {
                    me = (le.Body as UnaryExpression).Operand as MemberExpression;
                }
            }
            string rv = "";
            if (me != null)
            {
                rv = me.Member.Name;
            }
            while (me != null && getAll && me.NodeType == ExpressionType.MemberAccess)
            {
                Expression exp = me.Expression;
                if (exp is MemberExpression)
                {
                    rv = (exp as MemberExpression).Member.Name + "." + rv;
                    me = exp as MemberExpression;
                }
                else if (exp is MethodCallExpression)
                {
                    var mexp = exp as MethodCallExpression;
                    if (mexp.Method.Name == "get_Item")
                    {
                        var obj = ((mexp.Arguments[0] as MemberExpression).Expression as ConstantExpression).Value;
                        var index = obj.GetType().GetField((mexp.Arguments[0] as MemberExpression).Member.Name).GetValue(obj);
                        rv = (mexp.Object as MemberExpression).Member.Name + "[" + index + "]." + rv;
                        me = mexp.Object as MemberExpression;
                    }
                }
                else
                {
                    break;
                }
            }
            return rv;
        }

        public static string GetPropertyDisplayName(PropertyInfo pi, string fixedString = null)
        {
            if (!string.IsNullOrEmpty(fixedString))
            {
                //   return Utils.GetResourceText(fixedString);
            }
            if (fixedString == "")
            {
                return "";
            }
            string rv = "";
            var dis = pi.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            if (dis != null && !string.IsNullOrEmpty(dis.Name))
            {
                if (dis.ResourceType == null)
                {
                    rv = dis.Name;
                }
                else
                {
                    //  rv = Utils.GetResourceText(dis.Name, null, dis.ResourceType);
                }
            }
            else
            {
                rv = pi.Name;
            }
            return rv;
        }

        public static string GetPropertyDisplayName(Expression expression, string fixedString = null)
        {
            return PropertyHelper.GetPropertyDisplayName(PropertyHelper.GetPropertyInfo(expression), fixedString);
        }

        public static PropertyInfo GetPropertyInfo(Expression expression)
        {
            MemberExpression me = null;
            LambdaExpression le = null;
            if (expression is MemberExpression)
            {
                me = expression as MemberExpression;
            }
            if (expression is LambdaExpression)
            {
                le = expression as LambdaExpression;
                if (le.Body is MemberExpression)
                {
                    me = le.Body as MemberExpression;
                }
                if (le.Body is UnaryExpression)
                {
                    me = (le.Body as UnaryExpression).Operand as MemberExpression;
                }
            }
            PropertyInfo rv = null;
            if (me != null)
            {
                rv = me.Member.DeclaringType.GetProperty(me.Member.Name);
            }
            return rv;
        }

        public static string GetPropertyValue(LambdaExpression exp, object obj)
        {
            //获取表达式的值，并过滤单引号
            var expValue = exp.Compile().DynamicInvoke(obj);
            string val = "";
            if (expValue != null)
            {
                val = expValue.ToString().Replace("'", "\\\'").Replace(Environment.NewLine, "\\n").Replace("\n", "\\n");
            }
            return val;
        }

        public static string GetPropertyErrors<T>(HtmlHelper<T> html, Expression exp)
        {
            var expName = PropertyHelper.GetPropertyName(exp);
            return GetPropertyErrors(html, expName);
        }

        public static string GetPropertyErrors<T>(HtmlHelper<T> html, string property)
        {
            string expError = "";
            //获取表达式相关的错误信息
            if (html.ViewData.ModelState[property] != null)
            {
                var errors = html.ViewData.ModelState[property].Errors;
                if (errors != null && errors.Count > 0)
                {
                    //   expError = "[" + errors.ToSpratedString(x => x, (x) => { return "'" + (x.ErrorMessage == "" ? x.Exception.Message : x.ErrorMessage).Replace("'", "\\\'").Replace(Environment.NewLine, "\\n").Replace("\n", "\\n") + "'"; }) + "]";
                }
            }
            return expError;
        }

        public static bool IsPropertyRequired(PropertyInfo pi)
        {
            bool isRequired = false;
            //如果需要显示星号，则判断是否是必填项，如果是必填则在内容后面加上星号
            //所有int，float。。。这种Primitive类型的，肯定都是必填
            if (pi.PropertyType.IsPrimitive || pi.PropertyType.IsEnum)
            {
                isRequired = true;
            }
            else
            {
                //对于其他类，检查是否有RequiredAttribute，如果有就是必填
                var required = pi.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;
                if (required != null && required.AllowEmptyStrings == false)
                {
                    isRequired = true;
                }
                else
                {
                    //如果没有RequiredAttribute，检查是否有自定义的MLRequiredAttribute,如果有就是必填
                    //var mlrequired = pi.GetCustomAttributes(typeof(MLRequiredAttribute), false).FirstOrDefault() as MLRequiredAttribute;
                    //   if (mlrequired != null)
                    //{
                    //    isRequired = true;
                    //}
                }
            }
            return isRequired;
        }

        public static void SetPropertyValue(object source, string property, object value, string prefix = null, bool stringBasedValue = false)
        {
            try
            {
                List<string> level = new List<string>();
                if (property.Contains('.'))
                {
                    level.AddRange(property.Split('.'));
                }
                else
                {
                    level.Add(property);
                }

                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    level.Insert(0, prefix);
                }
                object temp = source;
                Type tempType = source.GetType();
                for (int i = 0; i < level.Count - 1; i++)
                {
                    var pro = tempType.GetProperty(level[i]);
                    if (pro != null)
                    {
                        var va = pro.GetValue(temp, null);
                        if (va != null)
                        {
                            temp = va;
                        }
                        else
                        {
                            var newInstance = va.GetType().GetConstructor(Type.EmptyTypes).Invoke(null);
                            pro.SetValue(temp, newInstance, null);
                            temp = newInstance;
                        }
                        tempType = pro.PropertyType;
                    }
                }

                var fproperty = tempType.GetProperty(level.Last());
                if (fproperty == null)
                {
                    return;
                }
                if (value == null)
                {
                    fproperty.SetValue(temp, value, null);
                }

                bool isArray = value != null && value.GetType().IsArray == true;

                if (stringBasedValue == true)
                {
                    Type propertyType = fproperty.PropertyType;
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var list = propertyType.GetConstructor(Type.EmptyTypes).Invoke(null) as IList;

                        var gs = propertyType.GenericTypeArguments;
                        try
                        {
                            if (isArray)
                            {
                                var a = (value as object[]);
                                for (int i = 0; i < a.Length; i++)
                                {
                                    list.Add(ConvertValue(a[i], gs[0]));
                                }
                            }
                            else
                            {
                                list = ConvertValue(value, propertyType) as IList;
                            }
                        }
                        catch { }
                        fproperty.SetValue(temp, list, null);
                    }
                    else
                    {
                        if (isArray)
                        {
                            var a = (value as object[]);
                            if (a.Length == 1)
                            {
                                value = a[0];
                            }
                        }

                        object val = ConvertValue(value, fproperty.PropertyType);
                        if (val is string)
                        {
                            val = val.ToString().Replace("\\", "/");
                        }
                        fproperty.SetValue(temp, val, null);
                    }
                }
                else
                {
                    if (value is string)
                    {
                        value = value.ToString().Replace("\\", "/");
                    }
                    fproperty.SetValue(temp, value, null);
                }
            }
            catch { }
        }

        private static object ConvertValue(object value, Type propertyType)
        {
            object val = null;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var gs = propertyType.GenericTypeArguments;
                try
                {
                    val = ConvertValue(value, gs[0]);
                }
                catch { }
            }
            else if (propertyType.IsEnum)
            {
                val = Enum.Parse(propertyType, value.ToString());
            }
            else if (propertyType == typeof(string))
            {
                val = value == null ? null : value.ToString().Trim();
            }
            else if (propertyType == typeof(Guid))
            {
                Guid g = Guid.Empty;
                Guid.TryParse(value.ToString(), out g);
                val = g;
            }
            else
            {
                try
                {
                    if (value.ToString().StartsWith("`") && value.ToString().EndsWith("`"))
                    {
                        string inner = value.ToString().Trim('`').TrimEnd(',');
                        if (!string.IsNullOrWhiteSpace(inner))
                        {
                            val = propertyType.GetConstructor(Type.EmptyTypes).Invoke(null);
                            string[] pair = inner.Split(',');
                            var gs = propertyType.GetGenericArguments();
                            foreach (var p in pair)
                            {
                                (val as IList).Add(Convert.ChangeType(p, gs[0]));
                            }
                        }
                    }
                    else
                    {
                        val = Convert.ChangeType(value.ToString(), propertyType);
                    }
                }
                catch
                {
                }
            }
            return val;
        }








        public static void SetPropertyValue(object source, string property, object value, bool stringBasedValue = false)
        {
            #region 针对于source 是 ViewModel

            var isViewModel = typeof(IBaseListViewModel<BaseEntity, BaseSearcher>).IsAssignableFrom(source.GetType());
            //var isViewModel = source.GetType().IsAssignableFrom(typeof(IBaseListViewModel<BaseEntity, BaseSearcher>));
            if (isViewModel)
            {
                property = property == "length" ? "Searcher.PageSize" : property;
                property = property == "start" ? "Searcher.StartRow" : property;
            }
            #endregion


            //是否属于xxxx.xxxx格式
            List<string> v = new List<string>();
            if (property.Contains('.'))
            {
                v.AddRange(property.Split('.'));
            }
            else
            {
                v.Add(property);
            }
            var temp = source;
            var tempType = source.GetType();
            for (int i = 0; i < v.Count - 1; i++)
            {
                var pro = tempType.GetProperty(v[i]);
                if (pro != null)
                {
                    var va = pro.GetValue(temp, null);
                    if (va != null)
                    {
                        temp = va;
                    }
                    else
                    {
                        var newInstance = va.GetType().GetConstructor(Type.EmptyTypes).Invoke(null);
                        pro.SetValue(temp, newInstance, null);
                        temp = newInstance;
                    }
                    tempType = pro.PropertyType;
                }
            }
            var fproperty = tempType.GetProperty(v.Last());
            if (fproperty == null)
            {
                return;
            }
            bool isArray = value.GetType().IsArray;
            if (stringBasedValue)
            {
                var propertyType = fproperty.PropertyType;
                if (propertyType.IsGenericType)
                {
                }
                else
                {
                    if (isArray)
                    {
                        var a = (value as object[]);
                        if (a != null && a.Length == 1)
                        {
                            value = a[0];
                        }
                    }
                    object val = ConvertValue(value, fproperty.PropertyType);
                    if (val is string)
                    {
                        val = val.ToString().Replace("\\", "/");
                    }
                    fproperty.SetValue(temp, val, null);
                }
            }
            else
            {
                if (value is string)
                {
                    value = value.ToString().Replace("\\", "/");
                }
                fproperty.SetValue(temp, value, null);
            }

        }
    }
}
