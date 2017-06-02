using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Common.Helpers.Extensions
{
    public static class DataContextExtension
    {


        public static IDataContext ReCreate(this IDataContext self)
        {
            var rv = (IDataContext)self.GetType().GetConstructor(Type.EmptyTypes).Invoke(null);
            self.Dispose();
            return rv;
        }







        public static List<SelectListItem> GetSelectListItems<T>(
            this IQueryable<T> baseQueryable,
            Expression<Func<T, bool>> whereCondition,
            Expression<Func<T, object>> textField,
            List<DataPrivilege> dataPrivileges,
            Expression<Func<T, object>> valueField = null,
            bool ignorDataPrivilege = false,
            bool sortByName = true) where T : BaseEntity
        {
            //var query = baseQueryable;
            //if (whereCondition != null)
            //    query = query.Where(whereCondition);
            //if (valueField == null)
            //    valueField = x => x.Id;
            //if (ignorDataPrivilege == false)
            //    query = AppendSelfDPWhere(query, dataPrivileges);
            //ParameterExpression pe = Expression.Parameter(typeof(T));
            //NewExpression newItem = Expression.New(typeof(SimpleTextAndValue));

            //var textMi = typeof(SimpleTextAndValue).GetMember("Text")[0];
            //ChangePara cp = new ChangePara();
            //MemberBinding textBind = Expression.Bind(textMi, cp.Change(textField.Body, pe));

            //var valueMi = typeof(SimpleTextAndValue).GetMember("Value")[0];
            //Expression valueFieldMember = Expression.PropertyOrField(pe, PropertyHelper.GetPropertyName(valueField, false));
            //MemberBinding valueBind = Expression.Bind(valueMi, valueFieldMember);
            return null;
        }


        private static IQueryable<T> AppendSelfDPWhere<T>(IQueryable<T> query, List<DataPrivilege> dataPrivileges) where T : BaseEntity
        {
            ParameterExpression pe = Expression.Parameter(typeof(T));
            Expression peid = Expression.PropertyOrField(pe, "ID");
            //循环数据权限，加入到where条件中，达到自动过滤的效果
            if (BaseController.DataPrivileges.SingleOrDefault(x => x.ModelName == query.ElementType.Name) != null)
            {
                //如果dps参数是空，则生成 1!=1 这种错误的表达式，这样就查不到任何数据了
                if (dataPrivileges == null)
                {
                    query = query.Where(Expression.Lambda<Func<T, bool>>(Expression.NotEqual(Expression.Constant(1), Expression.Constant(1)), pe));
                }
                else
                {
                    //在dps中找到和baseQuery源数据表名一样的关联id
                    var ids = dataPrivileges.Where(x => x.TableName == query.ElementType.Name).Select(x => x.RelateId).ToList();
                    if (ids == null || ids.Count() == 0)
                    {
                        query = query.Where(Expression.Lambda<Func<T, bool>>(Expression.NotEqual(Expression.Constant(1), Expression.Constant(1)), pe));
                    }
                    else
                    {
                        if (!ids.Contains(null))
                        {
                            List<Guid> finalIds = new List<Guid>();
                            ids.ForEach(x => finalIds.Add(x.Value));
                            Expression dpleft = Expression.Constant(finalIds, typeof(List<long>));
                            Expression dpcondition = Expression.Call(dpleft, "Contains", new Type[] { }, peid);
                            query = query.Where(Expression.Lambda<Func<T, bool>>(dpcondition, pe));
                        }
                    }
                }
            }
            return query;
        }
    }


    /// <summary>
    /// 简单键值对类，用来存著生成下拉菜单的数据
    /// </summary>
    public class SimpleTextAndValue
    {
        public object Text { get; set; }
        public object Value { get; set; }
    }

}
