using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Common
{
    public interface IBaseListViewModel<out TModel, out TSearch> where TModel : BaseEntity
        where TSearch : BaseSearcher
    {
        /// <summary>
        /// 获取列信息的数据
        /// </summary>
        /// <returns>Json格式的列信息</returns>
        string GetColumnsJson();
        /// <summary>
        /// 获取列信息的数据
        /// </summary>
        /// <returns>对象格式的列信息</returns>
        List<BootStrapTableColumn> GetColumnsObj();
        /// <summary>
        /// 获取数据以Json形式返回
        /// </summary>
        /// <returns></returns>
        string GetDataJson();
        /// <summary>
        /// 获取数据以HTML编码形式返回
        /// </summary>
        /// <returns></returns>
        string GetDataHtml();
        TSearch Searcher { get; }
        /// <summary>
        /// 是否需要分页
        /// </summary>
        bool NeedPage { get; set; }


    }
    public class BaseListViewModel<TModel, TSearch> : BaseViewModel, IBaseListViewModel<TModel, TSearch> where TModel : BaseEntity where TSearch : BaseSearcher
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseListViewModel()
        {
            NeedPage = true;
            length = 10;
            EntityList = new List<TModel>();
            //初始化搜索条件
            //Searcher = typeof(TSearch).GetConstructor(Type.EmptyTypes).Invoke(null) as TSearch;
            Searcher = Activator.CreateInstance(typeof(TSearch)) as TSearch;
        }
        #region 属性
        /// <summary>
        /// 搜索信息
        /// </summary>
        public TSearch Searcher { get; set; }
        /// <summary>
        /// 获取的数据
        /// </summary>
        public List<TModel> EntityList { get; set; }
        /// <summary>
        /// 查询模式
        /// </summary>
        public SearcherTypeEnum SearcherType { get; set; }
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
                //if (_listColumns.Where(x => x.Title.ToLower() == "id").FirstOrDefault() == null)
                //{
                //    //如果是QuickDebug模式，则显示ID列，否则将ID列隐藏
                //    if (BaseController.IsQuickDebug == true)
                //    {
                //        //_listColumns.Insert(0, this.MakeGridColumn(x => x.ID, null, "ID", Width: 50));
                //    }
                //    else
                //    {
                //        //_listColumns.Insert(0, this.MakeGridColumn(x => x.ID, null, "ID", Width: 0));
                //    }
                //}
                return _listColumns;
            }
            set
            {
                _listColumns = value;
            }
        }
        /// <summary>
        /// 是否需要分页
        /// </summary>
        public bool NeedPage { get; set; }
        /// <summary>
        /// 每行页数
        /// </summary>
        public int length { get; set; }
        public int draw { get; set; }
        #endregion
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
                //if (col.Children != null && col.Children.Count > 0)
                //{

                //}
                //else
                //{
                //sb.Append("[{\"Title\":" + col.Title + "");
                //}
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
        /// <summary>
        /// 获取列信息的数据
        /// </summary>
        /// <returns>Json格式的列信息</returns>
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
        #region 获取数据信息
        public string GetDataJson()
        {
            DoSearch();
            StringBuilder sb = new StringBuilder();
            var count = EntityList.Count;
            sb.Append("{");
            sb.Append($"\"draw\":{Searcher.Draw},");
            sb.Append($"\"recordsTotal\":{Searcher.TotalRecords},");
            sb.Append($"\"recordsFiltered\":{Searcher.TotalRecords},");
            sb.Append("\"data\":[");
            for (int i = 0; i < count; i++)
            {
                sb.Append(GetSingleDataJsonArray(EntityList[i]));
                if (i < EntityList.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]}");
            return sb.ToString();
        }
        public string GetSingleDataJsonNorml(TModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < ListColumns.Count; i++)
            {
                sb.Append($"\"{PropertyHelper.GetPropertyName(ListColumns[i].ColumnExp)}\":\"{ListColumns[i].ColumnExp.Compile()(model)}\"");
                if (i < ListColumns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
        public string GetSingleDataJsonArray(TModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < ListColumns.Count; i++)
            {
                sb.Append($"\"{ListColumns[i].ColumnExp.Compile()(model)}\"");
                if (i < ListColumns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }
        public string GetDataHtml()
        {
            DoSearch();
            StringBuilder sb = new StringBuilder();
            foreach (var item in EntityList)
            {
                sb.Append(GetSingleDataHtml(item));
            }
            return sb.ToString();
        }
        public string GetSingleDataHtml(TModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr role=\"row\" class=\"odd\">");
            foreach (var itemListColumn in ListColumns)
            {
                sb.Append($"<td textfiled=\"{PropertyHelper.GetPropertyName(itemListColumn.ColumnExp)}\">{itemListColumn.ColumnExp.Compile()(model)}</td>");
            }
            sb.Append("</tr>");
            return sb.ToString();
        }
        private void DoSearch()
        {
            IOrderedQueryable<TModel> query = null;
            //根据搜索模式调用不同的函数
            switch (SearcherType)
            {
                case SearcherTypeEnum.Search:
                    query = GetSearchQuery();
                    break;
                default:
                    query = GetSearchQuery();
                    break;
            }
            if (NeedPage && Searcher.PageSize != -1)
            {
                Searcher.TotalRecords = query.Count();
                EntityList = query.Skip(Searcher.StartRow).Take(Searcher.PageSize).AsNoTracking().ToList();
            }
            else
            {
                EntityList = query.AsNoTracking().ToList();
            }
        }
        #endregion
        public virtual IOrderedQueryable<TModel> GetSearchQuery()
        {
            return Dc.Set<TModel>().OrderByDescending(x => x.Id);
        }
        /// <summary>
        /// 初始化ListVM，继承的类应该重载这个函数来设定数据的列和动作
        /// </summary>
        protected virtual void InitListViewModel()
        {
            _listColumns = new List<IGridColumn<TModel>>();
        }
        public event Action<IBaseListViewModel<TModel, TSearch>> OnAfterInitList;
        /// <summary>
        /// 调用InitListVM并触发OnAfterInitList事件
        /// </summary>
        public void DoInitListViewModel()
        {
            InitListViewModel();
            if (OnAfterInitList != null)
            {
                OnAfterInitList(this);
            }
        }
    }
    public enum SearcherTypeEnum
    { Search }
}
