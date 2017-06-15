using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Common.Helpers;

namespace Sxxy_FrameworkArt.Common.SupportClasses
{

    /// <summary>
    /// 重复数据组
    /// </summary>
    /// <typeparam name="T">重复数据类</typeparam>
    public class DuplicatedGroup<T>
    {
        public List<DuplicatedField<T>> Fields { get; set; }
    }

    /// <summary>
    /// 重复数据信息
    /// </summary>
    /// <typeparam name="T">数据类</typeparam>
    public class DuplicatedInfo<T>
    {
        //重复数据分组
        public List<DuplicatedGroup<T>> Groups { get; set; }

        /// <summary>
        /// 创建重复数据信息
        /// </summary>
        /// <param name="FieldExps">一个或多个重复数据字段</param>
        /// <returns>重复数据信息</returns>
        public static DuplicatedInfo<T> CreateFieldInfo(params DuplicatedField<T>[] FieldExps)
        {
            //创建重复数据
            DuplicatedInfo<T> rv = new DuplicatedInfo<T>();
            //初始化组列表
            rv.Groups = new List<DuplicatedGroup<T>>();
            //添加一个新组
            rv.Groups.Add(new DuplicatedGroup<T>());
            //在新组中添加字段
            rv.Groups[0].Fields = new List<DuplicatedField<T>>();
            foreach (var exp in FieldExps)
            {
                rv.Groups[0].Fields.Add(exp);
            }
            return rv;
        }

        /// <summary>
        /// 添加新组
        /// </summary>
        /// <param name="FieldExps">一个或多个重复数据字段</param>
        public void AddGroup(params DuplicatedField<T>[] FieldExps)
        {
            DuplicatedGroup<T> newGroup = new DuplicatedGroup<T>();
            newGroup.Fields = new List<DuplicatedField<T>>();
            foreach (var exp in FieldExps)
            {
                newGroup.Fields.Add(exp);
            }
            Groups.Add(newGroup);
        }
    }

    /// <summary>
    /// 重复数据字段信息
    /// </summary>
    /// <typeparam name="T">重复数据类</typeparam>
    public class DuplicatedField<T>
    {
        //直接可顺序关联出的字段
        public Expression<Func<T, object>> DirectFieldExp { get; set; }

        /// <summary>
        /// 创建一个包含可顺序关联出字段的简单重复字段信息
        /// </summary>
        /// <param name="FieldExp">字段</param>
        /// <returns>字段信息</returns>
        public static DuplicatedField<T> SimpleField(Expression<Func<T, object>> FieldExp)
        {
            DuplicatedField<T> rv = new DuplicatedField<T>();
            rv.DirectFieldExp = FieldExp;
            return rv;
        }

    }

    /// <summary>
    /// 复杂重复数据字段信息
    /// </summary>
    /// <typeparam name="T">重复数据类</typeparam>
    /// <typeparam name="V">重复数据关联的List中的类</typeparam>
    public class ComplexDuplicatedField<T, V> : DuplicatedField<T>
    {
        /// <summary>
        /// 中间字段
        /// </summary>
        public Expression<Func<T, List<V>>> MiddleExp { get; set; }
        /// <summary>
        /// 最终字段
        /// </summary>
        public List<Expression<Func<V, object>>> SubFieldExps { get; set; }

        /// <summary>
        /// 创建一个复杂字段
        /// </summary>
        /// <param name="MiddleExp">中间字段类</param>
        /// <param name="FieldExps">最终字段类</param>
        /// <returns></returns>
        public static ComplexDuplicatedField<T, V> SubField(Expression<Func<T, List<V>>> MiddleExp, params Expression<Func<V, object>>[] FieldExps)
        {
            ComplexDuplicatedField<T, V> rv = new ComplexDuplicatedField<T, V>();
            rv.MiddleExp = MiddleExp;
            rv.SubFieldExps = new List<Expression<Func<V, object>>>();
            rv.SubFieldExps.AddRange(FieldExps);
            return rv;
        }

        /// <summary>
        /// 生成验证复杂字段是否重复的Lambda
        /// </summary>
        /// <param name="Entity">源数据</param>
        /// <param name="para">源数据类型</param>
        /// <returns>Where语句</returns>
        public Expression GetExpression(T Entity, ParameterExpression para)
        {
            ParameterExpression midPara = Expression.Parameter(typeof(V), "tm2");
            //获取中间表的List
            var list = MiddleExp.Compile().Invoke(Entity);

            List<Expression> allExp = new List<Expression>();
            Expression rv = null;
            //循环中间表数据
            foreach (var li in list)
            {
                List<Expression> innerExp = new List<Expression>();
                bool needBreak = false;
                //循环中间表要检查重复的字段
                foreach (var SubFieldExp in SubFieldExps)
                {
                    //拼接字段表达式，使left等于类似 x.field 的形式
                    Expression left = Expression.Property(midPara, PropertyHelper.GetPropertyName(SubFieldExp));
                    //如果字段是nullable类型的，则拼接value，形成类似 x.field.Value的形式
                    if (left.Type.IsGenericType && left.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        left = Expression.Property(left, "Value");
                    }
                    //如果字段是string类型，则拼接trim，形成类似 x.field.Trim()的形式
                    if (left.Type == typeof(string))
                    {
                        left = Expression.Call(left, typeof(String).GetMethod("Trim", Type.EmptyTypes));
                    }
                    typeof(String).GetMethod("Trim", Type.EmptyTypes).Invoke("",null);
                    //使用当前循环的中间表的数据获取字段的值
                    object vv = SubFieldExp.Compile().Invoke(li);
                    //如果值为空则跳过
                    if (vv == null)
                    {
                        needBreak = true;
                        continue;
                    }
                    //如果值为空字符串且没要求必填，则跳过
                    if (vv is string && vv.ToString() == "")
                    {
                        var requiredAttrs = li.GetType().GetProperty(PropertyHelper.GetPropertyName(SubFieldExp)).GetCustomAttributes(typeof(RequiredAttribute), false);

                        if (requiredAttrs == null || requiredAttrs.Length == 0)
                        {
                            needBreak = true;
                            continue;
                        }
                        else
                        {
                            var requiredAtt = requiredAttrs[0] as RequiredAttribute;
                            if (requiredAtt.AllowEmptyStrings == true)
                            {
                                needBreak = true;
                                continue;
                            }
                        }
                    }
                    //如果值为字符串，调用trim函数
                    if (vv is string)
                    {
                        vv = vv.ToString().Trim();
                    }
                    //拼接形成 x.field == value的形式
                    ConstantExpression right = Expression.Constant(vv);
                    BinaryExpression equal = Expression.Equal(left, right);
                    innerExp.Add(equal);
                }
                if (needBreak)
                {
                    continue;
                }
                //拼接多个 x.field==value，形成 x.field==value && x.field1==value1 .....的形式
                Expression exp = null;
                if (innerExp.Count == 1)
                {
                    exp = innerExp[0];
                }
                if (innerExp.Count > 1)
                {
                    exp = Expression.And(innerExp[0], innerExp[1]);
                    for (int i = 2; i < innerExp.Count; i++)
                    {
                        exp = Expression.And(exp, innerExp[i]);
                    }
                }
                //调用any函数，形成 .Any(x=> x.field==value && x.field1==value1....)的形式
                if (exp != null)
                {
                    var any = Expression.Call(
                        typeof(Enumerable),
                        "Any",
                        new Type[] { typeof(V) },
                        Expression.Property(para, PropertyHelper.GetPropertyName(MiddleExp)),
                        Expression.Lambda<Func<V, bool>>(exp, new ParameterExpression[] { midPara }));
                    allExp.Add(any);
                }
            }
            //拼接多个any函数形成 .Any(x=> x.field==value && x.field1==value1....) || .Any(x=> x.field==value && x.field1==value1....)的形式并返回
            if (allExp.Count == 1)
            {
                rv = allExp[0];
            }
            if (allExp.Count > 1)
            {
                rv = Expression.Or(allExp[0], allExp[1]);
                for (int i = 2; i < allExp.Count; i++)
                {
                    rv = Expression.Or(rv, allExp[i]);
                }
            }
            return rv;
        }

        /// <summary>
        /// 获取字段属性
        /// </summary>
        /// <returns>字段属性列表</returns>
        public List<PropertyInfo> GetProperties()
        {
            List<PropertyInfo> rv = new List<PropertyInfo>();
            foreach (var subField in SubFieldExps)
            {
                var pro = PropertyHelper.GetPropertyInfo(subField);
                if (pro != null)
                {
                    rv.Add(pro);
                }
            }
            return rv;
        }
    }
}

