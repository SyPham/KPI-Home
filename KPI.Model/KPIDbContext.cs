using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.EF;
namespace KPI.Model
{
    public class KPIDbContext : DbContext
    {
        const String DefaultConnectionName = "KPIDbContext";
        public KPIDbContext() : this(DefaultConnectionName)
        {
        }
        //public KPIDbContext() : base(DefaultConnectionName)
        //{
        //    this.Configuration.LazyLoadingEnabled = false;
        //}
        public KPIDbContext(String sqlConnectionName) : base(String.Format("Name={0}", sqlConnectionName))
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<EF.KPI> KPIs { get; set; }
        public DbSet<EF.KPILevel> KPILevels { get; set; }

        public DbSet<Data> Datas { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Level> Levels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<SeenComment> SeenComments { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet <Resource> Resources { get; set; }
        public DbSet <Permission> Permissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Unit> Units { get; set; }

        public DbSet<Revise> Revises { get; set; }
        public DbSet<ActionPlan> ActionPlans { get; set; }
        public DbSet <Notification> Notifications { get; set; }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            //builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId });
            //builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId);
        }
    }
}
