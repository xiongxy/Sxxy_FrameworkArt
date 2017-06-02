using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.DataAccess.Migrations;

namespace Sxxy_FrameworkArt.DataAccess
{
    public class DataContext: Sxxy_FrameworkArt.Common.DataContext
    {
        public DataContext()
            : base()
        {
            Database.SetInitializer<DataContext>(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
        }

        public DataContext(string cs)
            : base(cs)
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
