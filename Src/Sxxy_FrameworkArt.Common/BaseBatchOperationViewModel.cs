using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Common
{
    interface IBaseBatchOperationViewModel<out T> where T : BaseEntity
    {
        IEnumerable<T> Entitys { get; }
        void SetEntityByIds(IEnumerable<Guid> id);
        void CopyContext(BaseViewModel vm);
    }
    class BaseBatchOperationViewModel<TModel> : BaseViewModel, IBaseBatchOperationViewModel<TModel> where TModel : BaseEntity
    {
        public IEnumerable<TModel> Entitys { get; set; }
        public void SetEntityByIds(IEnumerable<Guid> id)
        {
            this.Entitys = GetByIds(id);
        }
        /// <summary>
        /// 根据主键获取Entity
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>Entity</returns>
        public virtual IEnumerable<TModel> GetByIds(IEnumerable<Guid> id)
        {
            //建立基础查询
            var query = Dc.Set<TModel>().AsQueryable();
            //获取数据
            var rv = query.Where(x => id.Contains(x.Id)).ToList();
            if (rv == null)
            {
                throw new ApplicationException("指定的数据不存在");
            }
            return rv;
        }
    }
}
