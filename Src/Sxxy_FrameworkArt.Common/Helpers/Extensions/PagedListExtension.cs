using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Common.Helpers.Extensions
{
    public static class PagedListExtension
    {
        /// <summary>
        /// 制作GridColumn
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TSearch"></typeparam>
        /// <param name="self"></param>
        /// <param name="columnExp"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public static GridColumn<TModel> MakeGridColumn<TModel, TSearch>(this IBaseListViewModel<TModel, TSearch> self, Expression<Func<TModel, object>> columnExp, bool isShow = true) where TModel : BaseEntity where TSearch : BaseSearcher
        {
            GridColumn<TModel> gridColumn = new GridColumn<TModel>(columnExp, isShow);
            return gridColumn;
        }
    }
}
