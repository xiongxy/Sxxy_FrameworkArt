using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Common.Helpers;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models;

namespace Sxxy_FrameworkArt.Common
{
    interface IBaseCrudViewModel<out T> where T : BaseEntity
    {

        T Entity { get; }
        /// <summary>
        /// 根据主键ID获取Entity
        /// </summary>
        /// <param name="id">主键ID</param>
        void SetEntityById(Guid id);
        /// <summary>
        /// 根据主键ID获取Entitys
        /// </summary>
        /// <param name="ids">主键IDs</param>
        void SetEntityByIds(IEnumerable<Guid> ids);

        /// <summary>
        /// 设置Entity
        /// </summary>
        /// <param name="entity">要设定的BasePoco</param>
        void SetEntity(object entity);

        /// <summary>
        /// 添加
        /// </summary>
        void DoAdd();

        /// <summary>
        /// 修改
        /// </summary>
        void DoEdit();

        /// <summary>
        /// 删除，对于BasePoco进行物理删除，对于PersistPoco把IsValid修改为false
        /// </summary>
        void DoDelete();

        /// <summary>
        /// 彻底删除，对PersistPoco进行物理删除
        /// </summary>
        void DoRealDelete();

        /// <summary>
        /// 将源VM的上数据库上下文，Session，登录用户信息，模型状态信息，缓存信息等内容复制到本VM中
        /// </summary>
        /// <param name="vm">复制的源</param>
        void CopyContext(BaseViewModel vm);
    }

    public class BaseCrudViewModel<TModel> : BaseViewModel, IBaseCrudViewModel<TModel> where TModel : BaseEntity
    {
        public IList<TModel> EntityList { get; set; }
        public TModel Entity { get; set; }

        //保存读取时Include的内容
        private List<Expression<Func<TModel, object>>> _toInclude { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseCrudViewModel()
        {
            //初始化Entity
            var ctor = typeof(TModel).GetConstructor(Type.EmptyTypes);
            Entity = ctor.Invoke(null) as TModel;
            //初始化VM中所有List<>的类
            var lists = typeof(TModel).GetProperties()
                .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(List<>));
            foreach (var li in lists)
            {
                var gs = li.PropertyType.GetGenericArguments();
                var newObj = Activator.CreateInstance(typeof(List<>).MakeGenericType(gs[0]));
                li.SetValue(Entity, newObj, null);
            }
        }

        public IQueryable<TModel> GetBaseQuery()
        {
            return Dc.Set<TModel>();
        }

        /// <summary>
        /// 设定添加和修改时对于重复数据的判断，子类进行相关操作时应重载这个函数
        /// </summary>
        /// <returns>唯一性属性</returns>
        public virtual DuplicatedInfo<TModel> SetDuplicatedCheck()
        {
            return null;
        }

        /// <summary>
        /// 设定读取是Include的内容，子类进行相关操作时应重载这个函数
        /// </summary>
        /// <param name="exps">需要关联的类</param>
        protected void SetInclude(params Expression<Func<TModel, object>>[] exps)
        {
            _toInclude = new List<Expression<Func<TModel, object>>>();
            _toInclude.AddRange(exps);
        }

        /// <summary>
        /// 根据主键ID设定Entity
        /// </summary>
        /// <param name="id">主键ID</param>
        public void SetEntityById(Guid id)
        {
            this.Entity = GetById(id);
        }

        public void SetEntityByIds(IEnumerable<Guid> ids)
        {

            this.EntityList = GetByIds(ids);
        }

        /// <summary>
        /// 设置Entity
        /// </summary>
        /// <param name="entity">要设定的BasePoco</param>
        public void SetEntity(object entity)
        {
            this.Entity = entity as TModel;
        }
        /// <summary>
        /// 根据主键获取Entity
        /// </summary>
        /// <param name="Id">主键ID</param>
        /// <returns>Entity</returns>
        public virtual TModel GetById(Guid Id)
        {
            TModel rv = null;
            //建立基础查询
            var query = Dc.Set<TModel>().AsQueryable();
            //如果BasePoco是多语言，则自动Include MLContents



            //循环添加其他设定的Include
            if (_toInclude != null)
            {
                foreach (var item in _toInclude)
                {
                    query = query.Include(item);
                }
            }
            //获取数据
            rv = query.SingleOrDefault(x => x.Id == Id);
            if (rv == null)
            {
                throw new ApplicationException("指定的数据不存在");
            }
            return rv;
        }
        public virtual IList<TModel> GetByIds(IEnumerable<Guid> Ids)
        {
            IList<TModel> rv = null;
            //建立基础查询
            var query = Dc.Set<TModel>().AsQueryable();
            //如果BasePoco是多语言，则自动Include MLContents
            //循环添加其他设定的Include
            if (_toInclude != null)
            {
                foreach (var item in _toInclude)
                {
                    query = query.Include(item);
                }
            }
            //获取数据
            rv = query.Where(x => Ids.Contains(x.Id)).ToList();
            if (rv == null)
            {
                throw new ApplicationException("指定的数据不存在");
            }
            return rv;
        }

        /// <summary>
        /// 添加，进行默认的添加操作。子类如有自定义操作应重载本函数
        /// </summary>
        public virtual void DoAdd()
        {
            //将所有BasePoco的属性赋空值，防止添加关联的重复内容
            var pros = typeof(TModel).GetProperties();
            foreach (var pro in pros)
            {
                if (pro.PropertyType.IsSubclassOf(typeof(BaseEntity)))
                {
                    pro.SetValue(Entity, null);
                }
            }
            //自动设定添加日期和添加人
            Entity.CreateTime = DateTime.Now;
            Entity.CreateBy = LoginUserInfo == null ? null : LoginUserInfo.Code;
            //添加数据
            Dc.Set<TModel>().Add(Entity);
            Dc.SaveChanges();
        }

        /// <summary>
        /// 修改，进行默认的修改操作。子类如有自定义操作应重载本函数
        /// </summary>
        public virtual void DoEdit()
        {
            //自动设定修改日期和修改人
            Entity.UpdateTime = DateTime.Now;
            Entity.UpdateBy = LoginUserInfo.Code;
            Dc.UpdateEntity(Entity);
            Dc.SaveChanges();
        }

        /// <summary>
        /// 删除，进行默认的删除操作。子类如有自定义操作应重载本函数
        /// </summary>
        public virtual void DoDelete()
        {
            //如果是PersistPoco，则把IsValid设为false，并不进行物理删除
            if (typeof(TModel).IsSubclassOf(typeof(PersistEntity)))
            {
                (Entity as PersistEntity).IsValid = false;
                Entity.UpdateTime = DateTime.Now;
                Entity.UpdateBy = LoginUserInfo.Code;
                Dc.UpdateEntity(Entity);
                try
                {
                    Dc.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    ModelStateDictionarys.AddModelError("", "该数据已被使用，无法删除");
                }
            }
            //如果是普通的BasePoco，则进行物理删除
            else if (typeof(TModel).IsSubclassOf(typeof(BaseEntity)))
            {
                DoRealDelete();
            }
        }

        /// <summary>
        /// 物理删除，对于普通的BasePoco和Delete操作相同，对于PersistPoco则进行真正的删除。子类如有自定义操作应重载本函数
        /// </summary>
        public virtual void DoRealDelete()
        {
            //删除本表
            Dc.DeleteEntity(Entity);
            try
            {
                Dc.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                ModelStateDictionarys.AddModelError("", "该数据已被使用，无法删除");
            }
        }

        /// <summary>
        /// 创建重复数据信息
        /// </summary>
        /// <param name="FieldExps">重复数据信息</param>
        /// <returns>重复数据信息</returns>
        protected DuplicatedInfo<TModel> CreateFieldsInfo(params DuplicatedField<TModel>[] FieldExps)
        {
            return DuplicatedInfo<TModel>.CreateFieldInfo(FieldExps);
        }

        /// <summary>
        /// 创建一个简单重复数据信息
        /// </summary>
        /// <param name="FieldExp">重复数据的字段</param>
        /// <returns>重复数据信息</returns>
        public static DuplicatedField<TModel> SimpleField(Expression<Func<TModel, object>> FieldExp)
        {
            return DuplicatedField<TModel>.SimpleField(FieldExp);
        }

        /// <summary>
        /// 创建一个关联到其他表数组中数据的重复信息
        /// </summary>
        /// <typeparam name="V">关联表类</typeparam>
        /// <param name="MiddleExp">指向关联表类数组的Lambda</param>
        /// <param name="FieldExps">指向最终字段的Lambda</param>
        /// <returns>重复数据信息</returns>
        public static DuplicatedField<TModel> SubField<V>(Expression<Func<TModel, List<V>>> MiddleExp,
            params Expression<Func<V, object>>[] FieldExps)
        {
            return ComplexDuplicatedField<TModel, V>.SubField(MiddleExp, FieldExps);
        }

        /// <summary>
        /// 验证数据，默认验证多语言数据以及重复数据。子类如需要其他自定义验证，则重载这个函数
        /// </summary>
        /// <param name="validationContext">验证环境</param>
        /// <returns>验证结果</returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> rv = new List<ValidationResult>();
            //验证重复数据
            ValidateDuplicateData(rv);
            return rv;
        }

        /// <summary>
        /// 验证重复数据
        /// </summary>
        /// <param name="rv">验证结果</param>
        protected void ValidateDuplicateData(List<ValidationResult> rv)
        {
            //获取设定的重复字段信息
            var checkCondition = SetDuplicatedCheck();
            if (checkCondition != null && checkCondition.Groups.Count > 0)
            {
                //生成基础Query
                var baseExp = Dc.Set<TModel>().AsQueryable();
                var modelType = typeof(TModel);
                ParameterExpression para = Expression.Parameter(modelType, "tm");
                //循环所有重复字段组
                foreach (var group in checkCondition.Groups)
                {
                    List<Expression> conditions = new List<Expression>();
                    //生成一个表达式，类似于 x=>x.ID != id，这是为了当修改数据时验证重复性的时候，排除当前正在修改的数据
                    MemberExpression idLeft = Expression.Property(para, "ID");
                    ConstantExpression idRight = Expression.Constant(Entity.Id);
                    BinaryExpression idNotEqual = Expression.NotEqual(idLeft, idRight);
                    conditions.Add(idNotEqual);
                    List<PropertyInfo> props = new List<PropertyInfo>();
                    //在每个组中循环所有字段
                    foreach (var field in group.Fields)
                    {
                        //如果是直接的简单字段
                        if (field.DirectFieldExp != null)
                        {
                            var item = field.DirectFieldExp;
                            string propertyName = PropertyHelper.GetPropertyName(item);
                            var prop = PropertyHelper.GetPropertyInfo(item);
                            //将字段名保存，为后面生成错误信息作准备
                            props.Add(prop);
                            var func = item.Compile();
                            var val = func.Invoke(this.Entity);

                            //如果字段值为null则跳过，因为一般情况下null值不会被认为重复
                            if (val == null)
                            {
                                continue;
                            }
                            //如果字段值是空字符串，则跳过
                            if (val is string && val.ToString() == "")
                            {
                                var requiredAttrs = prop.GetCustomAttributes(typeof(RequiredAttribute), false);

                                if (requiredAttrs == null || requiredAttrs.Length == 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    var requiredAtt = requiredAttrs[0] as RequiredAttribute;
                                    if (requiredAtt.AllowEmptyStrings == true)
                                    {
                                        continue;
                                    }
                                }
                            }
                            //生成一个表达式，类似于 x=>x.field == val
                            string[] splits = propertyName.Split('.');
                            Expression left = Expression.PropertyOrField(para, splits[0]);
                            for (int i = 1; i < splits.Length; i++)
                            {
                                left = Expression.PropertyOrField(left, splits[i]);
                            }

                            if (left.Type.IsGenericType && left.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                left = Expression.Property(left, "Value");
                            }
                            if (left.Type == typeof(string))
                            {
                                left = Expression.Call(left, typeof(String).GetMethod("Trim", Type.EmptyTypes));
                            }
                            if (val is string)
                            {
                                val = val.ToString().Trim();
                            }
                            ConstantExpression right = Expression.Constant(val);
                            BinaryExpression equal = Expression.Equal(left, right);
                            conditions.Add(equal);
                        }
                        //如果是复杂字段，则调用ComplexDuplicatedField类中的GetExpression来生成表达式
                        else
                        {
                            dynamic dField = field;
                            Expression exp = dField.GetExpression(Entity, para);
                            if (exp != null)
                            {
                                conditions.Add(exp);
                            }
                            //将字段名保存，为后面生成错误信息作准备
                            props.AddRange(dField.GetProperties());
                        }
                    }
                    int count = 0;
                    if (conditions.Count > 1)
                    {
                        //循环添加条件并生成Where语句
                        Expression conExp = conditions[0];
                        for (int i = 1; i < conditions.Count; i++)
                        {
                            conExp = Expression.And(conExp, conditions[i]);
                        }

                        MethodCallExpression whereCallExpression = Expression.Call(
                            typeof(Queryable),
                            "Where",
                            new Type[] { modelType },
                            baseExp.Expression,
                            Expression.Lambda<Func<TModel, bool>>(conExp, new ParameterExpression[] { para }));
                        var result = baseExp.Provider.CreateQuery(whereCallExpression);
                        foreach (var res in result)
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        //循环拼接所有字段名
                        string AllName = "";
                        foreach (var prop in props)
                        {
                            string name = PropertyHelper.GetPropertyDisplayName(prop);
                            AllName += name + ",";
                        }
                        if (AllName.EndsWith(","))
                        {
                            AllName = AllName.Remove(AllName.Length - 1);
                        }
                        //如果只有一个字段重复，则拼接形成 xxx字段重复 这种提示
                        if (props.Count == 1)
                        {
                            rv.Add(new ValidationResult(AllName + "字段存在重复数据", GetValidationFieldName(props[0])));
                        }
                        //如果多个字段重复，则拼接形成 xx，yy，zz组合字段重复 这种提示
                        else if (props.Count > 1)
                        {
                            rv.Add(new ValidationResult(AllName + "的字段组合存在重复数据",
                                GetValidationFieldName(props.First())));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 根据属性信息获取验证字段名
        /// </summary>
        /// <param name="pi">属性信息</param>
        /// <returns>验证字段名称数组，用于ValidationResult</returns>
        private string[] GetValidationFieldName(PropertyInfo pi)
        {
            return new[] { "Entity." + pi.Name };
        }


    }
}
