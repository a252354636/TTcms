using System.Data.Entity;
using TTcms.Domain.System;

namespace TTcms.EFRepositories
{
    partial class MyDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<App> Apps { get; set; }

        public DbSet<Role> Roles { get; set; }

        private void MapSystemModuleEntities(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Sys_User");
        }
    }
}
