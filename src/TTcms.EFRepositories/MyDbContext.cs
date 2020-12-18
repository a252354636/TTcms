using System;
using System.Data.Entity;
using System.Reflection;
using Wechart.EFRepositories.Migrations;

namespace TTcms.EFRepositories
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("TTcmsDB")
        {

            //this.Database.Log = LogUtil.Debug;
            //Database.SetInitializer<MyDbContext>(null);
            Database.SetInitializer(new MyDbContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            MapSystemModuleEntities(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
