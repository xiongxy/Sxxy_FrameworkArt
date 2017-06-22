using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Common
{
    public interface IBaseListViewModel<out TModel, out TSearch>
    {
        /// <summary>
        /// 获取列信息的数据
        /// </summary>
        /// <returns>Json格式的列信息</returns>
        string GetColumnsJson();
        TSearch Searcher { get; }
        List<BootStrapTableColumn> GetColumnsObj();
    }
    public class BaseListViewModel<TModel, TSearch> : BaseViewModel, IBaseListViewModel<TModel, TSearch> where TModel : BaseEntity where TSearch : BaseSearcher
    {
        public BaseListViewModel()
        {
            //初始化搜索条件
            //Searcher = typeof(TSearch).GetConstructor(Type.EmptyTypes).Invoke(null) as TSearch;
            Searcher = Activator.CreateInstance(typeof(TSearch)) as TSearch;
        }

        #region 获取列信息
        /// <summary>
        /// 获取列信息的数据
        /// </summary>
        /// <returns>Json格式的列信息</returns>
        public string GetColumnsJson()
        {
            int count = 0;
            //调用递归函数
            return GetColumnsJson(ref count);
        }

        private string GetColumnsJson(ref int count, List<IGridColumn<TModel>> basecols = null)
        {
            StringBuilder sb = new StringBuilder();
            var cols = basecols;
            if (cols == null)
            {
                cols = this.ListColumns;
            }
            //循环所有列
            for (int i = 0; i < cols.Count; i++)
            {
                var col = cols[i];
                //获取列字段的类型
                Type ptype = null;
                if (col.ColumnExp != null)
                {
                    ptype = PropertyHelper.GetPropertyInfo(col.ColumnExp).PropertyType;
                }
                //如果有子列，则递归调用自己，生成json
                if (col.Children != null && col.Children.Count > 0)
                {

                }
                else
                {
                    sb.Append("[{\"Title\":" + col.Title + "");
                }
                if (i < cols.Count - 1)
                {
                    sb.Append("},");
                }
                else
                {
                    sb.Append("}");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public List<BootStrapTableColumn> GetColumnsObj()
        {
            List<BootStrapTableColumn> list = new List<BootStrapTableColumn>();
            foreach (var itemListColumn in ListColumns)
            {
                BootStrapTableColumn v = new BootStrapTableColumn();
                v.Title = itemListColumn.Title;
                list.Add(v);
            }
            return list;
        }
        #endregion
        /// <summary>
        /// 初始化ListVM，继承的类应该重载这个函数来设定数据的列和动作
        /// </summary>
        protected virtual void InitListViewModel()
        {
            _listColumns = new List<IGridColumn<TModel>>();
        }
        private List<IGridColumn<TModel>> _listColumns;
        /// <summary>
        /// 数据列
        /// </summary>
        public List<IGridColumn<TModel>> ListColumns
        {
            get
            {
                //如果没有列信息，则调用Init方法
                if (_listColumns == null)
                {
                    DoInitListViewModel();
                }
                if (_listColumns.Where(x => x.Title.ToLower() == "id").FirstOrDefault() == null)
                {
                    //如果是QuickDebug模式，则显示ID列，否则将ID列隐藏
                    if (BaseController.IsQuickDebug == true)
                    {
                        //_listColumns.Insert(0, this.MakeGridColumn(x => x.ID, null, "ID", Width: 50));
                    }
                    else
                    {
                        //_listColumns.Insert(0, this.MakeGridColumn(x => x.ID, null, "ID", Width: 0));
                    }
                }
                return _listColumns;
            }
            set
            {
                _listColumns = value;
            }
        }
        public TSearch Searcher { get; set; }
        public event Action<IBaseListViewModel<TModel, TSearch>> OnAfterInitList;
        /// <summary>
        /// 调用InitListVM并触发OnAfterInitList事件
        /// </summary>
        public void DoInitListViewModel()
        {
            this.InitListViewModel();
            if (OnAfterInitList != null)
            {
                OnAfterInitList(this);
            }
        }
    }
}
