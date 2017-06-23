using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sxxy_FrameworkArt.Models
{
    [Serializable]
    public abstract class TopBaseEntity
    {
        private Guid _id;

        [Key]
        public virtual Guid Id
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
    /// <summary>
    /// Entity 的基类，所有的entity对象都应该继承这个类，这样会使所有的Entity 对应的数据表都有一个主键
    /// </summary>
    public abstract class BaseEntity : TopBaseEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }
    }
    /// <summary>
    /// Entity 的基类，不需要物理删除的都应该继承这个类，这样会使所有的Entity 对应的数据表都有一个IsValid
    /// </summary>
    public abstract class PersistEntity : BaseEntity
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }
    }



    //public abstract class BaseParent<TParent>
    //{
    //    public Guid ParentId { get; set; }
    //    public TParent Parent { get; set; }
    //}

    #region 其他 other
    /// <summary>
    /// 树形结构model接口
    /// </summary>
    public interface ITreeData
    {
        //父节点ID
        Guid? ParentId { get; set; }
    }
    /// <summary>
    /// 泛型树形结构model接口
    /// </summary>
    /// <typeparam name="T">父节点类型，父节点应该和实现本接口的类是同一个类，否则没有意义</typeparam>
    public interface ITreeData<out T> : ITreeData where T : BaseEntity
    {
        /// <summary>
        /// 通过实现这个函数获得所有的子节点数据
        /// </summary>
        /// <returns>所有子节点数据</returns>
        IEnumerable<T> GetChildren();
        //获取父节点
        T Parent { get; }
    }


    #endregion
}