using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Common.SupportClasses
{

    public interface IGridColumn<T>
    {
        #region 属性 Properties
        /// <summary>
        /// 列标题
        /// </summary>
        //string Title { get; set; }
        ///// <summary>
        ///// 是否需要分组
        ///// </summary>
        //bool IsNeedGroup { get; set; }
        ///// <summary>
        ///// 列宽
        ///// </summary>
        //int? Width { get; set; }
        ///// <summary>
        ///// 是否允许多行
        ///// </summary>
        //bool IsAllowMultiLine { get; set; }
        ///// <summary>
        ///// 最大子列数量
        ///// </summary>
        //int MaxChildrenCount { get; }
        ///// <summary>
        ///// 多表头的最大层数
        ///// </summary>
        //int MaxLevel { get; }
        ///// <summary>
        ///// 是否填充
        ///// </summary>
        //bool Flex { get; set; }
        ///// <summary>
        ///// 格式化函数
        ///// </summary>
        //Func<T, dynamic, object> Format { get; set; }
        /// <summary>
        /// 列表达式
        /// </summary>
        Expression<Func<T, object>> ColumnExp { get; set; }
        ///// <summary>
        ///// 子列
        ///// </summary>
        //List<IGridColumn<T>> Children { get; }
        ///// <summary>
        ///// 最下层列
        ///// </summary>
        //List<IGridColumn<T>> BottomChildren { get; }
        #endregion
        #region 方法 Function
        ///// <summary>
        ///// 获取内容
        ///// </summary>
        ///// <param name="source">源数据</param>
        ///// <returns>内容</returns>
        //System.Web.Mvc.MvcHtmlString GetText(T source);
        ///// <summary>
        ///// 获取前景色
        ///// </summary>
        ///// <param name="source">源数据</param>
        ///// <returns>前景色</returns>
        //System.Drawing.Color GetForeGroundColor(T source);
        ///// <summary>
        ///// 获取背景色
        ///// </summary>
        ///// <param name="source">源数据</param>
        ///// <returns>背景色</returns>
        //System.Drawing.Color GetBackGroundColor(T source);
        #endregion
    }

    public class GridColumn<T> : IGridColumn<T> where T : BaseEntity
    {
        public GridColumn()
        {

        }
        public GridColumn(Expression<Func<T, object>> columnExp)
        {
            this.ColumnExp = columnExp;
        }
        
        //public List<IGridColumn<T>> BottomChildren { get; set; }

        //public List<IGridColumn<T>> Children { get; set; }

        public Expression<Func<T, object>> ColumnExp { get; set; }

        //public bool Flex { get; set; }

        //public Func<T, dynamic, object> Format { get; set; }

        //public bool IsAllowMultiLine { get; set; }

        //public bool IsNeedGroup { get; set; }

        //public int MaxChildrenCount { get; set; }

        //public int MaxLevel { get; set; }
        //private string _title { get; set; }

        //public string Title
        //{
        //    get
        //    {
        //        if (_title == null)
        //        {
        //            return GetTitle();
        //        }
        //        return _title;
        //    }
        //    set { _title = value; }
        //}

        /// <summary>
        /// 获取标题内容
        /// </summary>
        /// <returns></returns>
        private string GetTitle()
        {
            string str = PropertyHelper.GetPropertyDisplayName(this.ColumnExp);
            if (str != null)
            {
                return str;
            }
            return "";
        }

        //public int? Width { get; set; }

        //public Color GetBackGroundColor(T source)
        //{
        //    return Color.Coral;
        //}

        //public Color GetForeGroundColor(T source)
        //{
        //    return Color.Coral;
        //}

        //public MvcHtmlString GetText(T source)
        //{
        //    return new MvcHtmlString("");
        //}
    }
}
