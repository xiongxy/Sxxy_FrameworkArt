using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sxxy_FrameworkArt.Models;
using Sxxy_FrameworkArt.Models.SystemEntity;
using Sxxy_FrameworkArt.Common.Helpers.Extensions;

namespace Sxxy_FrameworkArt.Common.SupportClasses
{

    public interface IDataPrivilege
    {
        string ModelName { get; set; }
        string PrivilegeName { get; set; }
        List<SelectListItem> GetItemList(IDataContext dc, List<DataPrivilege> dps);
    }

    public class DataPrivilegeInfo<T> : IDataPrivilege where T : BaseEntity
    {
        public DataPrivilegeInfo(string name, Expression<Func<T, object>> displayField, Expression<Func<T, bool>> @where)
        {
            ModelName = typeof(T).Name;
            PrivilegeName = name;
            _displayField = displayField;
            _where = @where;
        }

        public string ModelName { get; set; }

        public string PrivilegeName { get; set; }

        //显示字段
        private readonly Expression<Func<T, object>> _displayField;
        //where过滤条件
        private readonly Expression<Func<T, bool>> _where;


        List<SelectListItem> IDataPrivilege.GetItemList(IDataContext dc, List<DataPrivilege> dps)
        {
            List<SelectListItem> rv = new List<SelectListItem>();
            rv = dc.Set<T>().GetSelectListItems(_where, _displayField, dps, ignorDataPrivilege: true);
            return rv;
        }
    }


}
