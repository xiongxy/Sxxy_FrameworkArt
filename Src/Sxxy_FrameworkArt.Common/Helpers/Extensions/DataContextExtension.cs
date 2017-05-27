using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Models;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Common.Helpers.Extensions
{
    public static class DataContextExtension
    {
        public static List<SelectListItem> GetSelectListItems<T>(this IQueryable<T> baseQueryable, Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> textField, List<DataPrivilege> dataPrivileges, Expression<Func<T, object>> valueField = null, bool ignorDataPrivilege = false, bool sortByName = true) where T : BaseEntity
        {
            var query = baseQueryable;

        }
    }
}
