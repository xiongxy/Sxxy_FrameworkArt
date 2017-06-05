using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sxxy_FrameworkArt.Common
{
    /// <summary>
    /// 所有ViewModel的基类，提供了基本的功能
    /// </summary>
    public class BaseViewModel
    {
        /// <summary>
        /// 初始化ViewModel，框架会在创建VM实例之后自动调用本函数
        /// </summary>
        protected virtual void InitViewModel()
        {
        }
        /// <summary>
        /// 从新初始化ViewModel，框架会在验证失败时自动调用本函数
        /// </summary>
        protected virtual void ReInitViewModel()
        {
            InitViewModel();
        }

        private IDataContext _dc;
        /// <summary>
        /// 数据库环境
        /// </summary>
        public IDataContext Dc
        {
            get
            {
                if (_dc == null)
                {
                    _dc = (IDataContext)BaseController.DefaultDataContext.GetConstructor(Type.EmptyTypes).Invoke(null);
                }
                return _dc;
            }
            set
            {
                _dc = value;
            }
        }

        /// <summary>
        /// 模型状态，可在ViewModel中添加验证错误信息
        /// </summary>
        public ModelStateDictionary ModelStateDictionarys { get; set; }
    }
}
