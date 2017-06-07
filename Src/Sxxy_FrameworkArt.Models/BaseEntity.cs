using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models
{
    /// <summary>
    /// Entity 的基类，所有的entity对象都应该继承这个类，这样会使所有的Entity 对应的数据表都有一个主键
    /// </summary>
    public class BaseEntity : TopBaseEntity
    {

        public DateTime? CreateTime { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UpdateBy { get; set; }
    }
    public class TopBaseEntity
    {
        private Guid _id;
        [Key]
        public Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                {
                    _id = Guid.NewGuid();
                }
                return _id;

            }
            set
            {
                _id = value;
            }
        }
    }
    public class PersistEntity : BaseEntity
    {
        //是否有效
        public bool IsValid { get; set; }
    }
}


//public abstract class BaseParent<TParent>
//{
//    public Guid ParentId { get; set; }
//    public TParent Parent { get; set; }
//}
