using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Common
{
    /// <summary>
    /// 所有ViewModel的基类，提供了基本的功能
    /// </summary>
    public class BaseViewModel : IValidatableObject
    {
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

        public string NowControllerName { get; set; }

        /// <summary>
        /// 菜单(属性)
        /// </summary>
        public List<SystemMenu> SystemMenuProperty
        {
            get { return BaseController.SystemMenuProperty; }
        }
        public HttpSessionStateBase Session { get; set; }
        public LoginUserInfo LoginUserInfo
        {
            get
            {
                if (Session == null || Session["UserInfo"] == null)
                {
                    return null;
                }
                return Session["UserInfo"] as LoginUserInfo;
            }
            set
            {
                Session["UserInfo"] = value;
            }
        }
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
        /// <summary>
        /// 模型状态，可在ViewModel中添加验证错误信息
        /// </summary>
        public ModelStateDictionary ModelStateDictionarys { get; set; }
        /// <summary>
        /// 记录Controller中的表单数据
        /// </summary>
        public FormCollection InSideFormCollection { get; set; }
        public void CopyContext(BaseViewModel vm)
        {
            this.Dc = vm.Dc;
            //this.Session = vm.Session;
            //this.LoginUserInfo = vm.LoginUserInfo;
            this.ModelStateDictionarys = vm.ModelStateDictionarys;
        }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> rv = new List<ValidationResult>();
            return rv;
        }
    }
}
