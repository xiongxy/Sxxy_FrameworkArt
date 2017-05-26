using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Models;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Common
{
    public interface IDataContext : IDisposable
    {
        bool IsFake { get; set; }
        void AddEntity(BaseEntity entity);
        void UpdateEntity(BaseEntity entity);
        void UpdateProperty<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> fidldexp)
            where TEntity : BaseEntity;
        void DeleteEntity<TEntity>(TEntity entity) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Database Database { get; }
        void ChangeRelationTo<T, V>(T source, Expression<Func<T, List<V>>> navigation, List<long> oldIDs, List<long> newIDs)
            where T : BaseEntity
            where V : BaseEntity;
    }
    public class DataContext : DbContext, IDataContext
    {
        #region DbSet
        public DbSet<SystemModule> SystemModules { get; set; }
        public DbSet<SystemDomain> SystemDomains { get; set; }
        public DbSet<SystemAction> SystemActions { get; set; }
        public DbSet<FunctionPrivilege> FunctionPrivileges { get; set; }
        public DbSet<SystemMenu> SystemMenus { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        #endregion


        public DataContext() : base(GetConnstring())
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<DataContext>(null);

        }

        public DataContext(string connstring)
            : base(connstring)
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<DataContext>(null);
        }
        bool IDataContext.IsFake { get; set; }

        void IDataContext.AddEntity(BaseEntity entity)
        {
            this.Entry(entity).State = EntityState.Added;
        }

        void IDataContext.UpdateEntity(BaseEntity entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        void IDataContext.UpdateProperty<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> fidldexp)
        {
            this.Entry(entity).Property(fidldexp).IsModified = true;
        }
        void IDataContext.DeleteEntity<TEntity>(TEntity entity)
        {
            var set = this.Set<TEntity>();
            if (!set.Local.Contains(entity))
            {
                set.Attach(entity);
            }
            set.Remove(entity);
        }
        public static string GetConnstring()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["Connstring"].ConnectionString;
        }
        void IDataContext.ChangeRelationTo<T, V>(T source, Expression<Func<T, List<V>>> navigation, List<long> oldIDs, List<long> newIDs)
        {
            throw new NotImplementedException();
        }
    }
}
