using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;
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
        /// 获取查询数据以Json形式返回
        /// </summary>
        /// <returns></returns>
        string GetDataJson();
        /// <summary>
        /// 获取查询数据以HTML编码形式返回
        /// </summary>
        /// <returns></returns>
        string GetDataHtml();
        /// <summary>
        /// 获取树形数据以HTML编码形式返回
        /// </summary>
        /// <returns></returns>
        string GetTreeDataJson();
        TSearch Searcher { get; }
        /// <summary>
        /// 是否需要分页
        /// </summary>
        bool NeedPage { get; set; }
        List<GridAction> GridActions { get; set; }
        /// <summary>
        /// 获取动作的Json格式数据
        /// </summary>
        /// <returns></returns>
        string GetActionJson();
    }
    public class BaseListViewModel<TModel, TSearch> : BaseViewModel, IBaseListViewModel<TModel, TSearch> where TModel : BaseEntity where TSearch : BaseSearcher
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseListViewModel()
        {
            NeedPage = true;
            EntityList = new List<TModel>();
            //初始化搜索条件
            Searcher = Activator.CreateInstance(typeof(TSearch)) as TSearch;
            GridActions = new List<GridAction>();
        }
        #region 属性
        public List<GridAction> GridActions { get; set; }

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
                if (ListColumns[i] is GridActionColumn<TModel>)
                {
                    //sb.Append("\"<a class='glyphicon glyphicon-pencil' style='font-size:20px;color:black;margin-left:10px;'></a>");
                    //sb.Append("<a class='glyphicon glyphicon-trash' style='font-size:20px;color:black;;margin-left:10px;'></a>\"");
                    sb.Append("\"\"");
                }
                else
                {
                    sb.Append($"\"{ListColumns[i].ColumnExp.Compile()(model)}\"");
                    if (i < ListColumns.Count - 1)
                    {
                        sb.Append(",");
                    }
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
        public string GetTreeDataJson()
        {
            DoSearch();
            StringBuilder sb = new StringBuilder();
            StringBuilder tempsb = new StringBuilder();
            var count = EntityList.Count;
            GetTreeDataReordering();
            sb.Append("{");
            sb.Append($"\"draw\":{Searcher.Draw},");
            sb.Append($"\"recordsTotal\":{Searcher.TotalRecords},");
            sb.Append($"\"recordsFiltered\":{Searcher.TotalRecords},");
            sb.Append("\"data\":[");
            for (int i = 0; i < count; i++)
            {
                sb.Append(GetTreeSingleDataJsonArray(EntityList[i]));
                if (i < EntityList.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]}");
            return sb.ToString();
        }
        public void GetTreeDataReordering()
        {
            IEnumerable<dynamic> temp = EntityList;
            EntityList = new List<TModel>();
            if (temp != null)
            {
                //最上级的数据
                var v = temp.Where(x => x.ParentId == null).ToList();
                foreach (var item in v)
                {
                    EntityList.Add(item);
                    GetTreeDataReorderingRecursion(temp, item);
                }
            }
        }

        public void GetTreeDataReorderingRecursion(IEnumerable<dynamic> temp, dynamic thisObjects)
        {
            var v1 = temp.Where(x => x.ParentId == thisObjects.Id).ToList();
            foreach (var item in v1)
            {
                EntityList.Add(item);
                GetTreeDataReorderingRecursion(temp, item);
            }
        }
        private string GetTreeSingleDataJsonArray(TModel model)
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

        public string GetActionJson()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in GridActions)
            {

            }
            return sb.ToString();
        }
    }
    public enum SearcherTypeEnum
    { Search }
}
