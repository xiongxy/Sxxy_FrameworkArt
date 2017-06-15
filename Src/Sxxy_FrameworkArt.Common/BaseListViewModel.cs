using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Common.SupportClasses;

namespace Sxxy_FrameworkArt.Common
{
    public interface IBaseListViewModel<out TModel, out TSearch>
    {
    }
    public class BaseListViewModel<TModel, TSearch> : BaseViewModel, IBaseListViewModel<TModel, TSearch>
    {
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
                    DoInitListVM();
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
        public event Action<IBaseListViewModel<TModel, TSearch>> OnAfterInitList;
        /// <summary>
        /// 调用InitListVM并触发OnAfterInitList事件
        /// </summary>
        public void DoInitListVM()
        {
            InitListViewModel();
            if (OnAfterInitList != null)
            {
                OnAfterInitList(this);
            }
        }
    }
}
