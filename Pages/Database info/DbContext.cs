using DrochClicker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Drochclicker.Pages.Database_info
    
{
    public class DbContextInfo : DbContext
    {
        public DbContextInfo(DbContextOptions<DbContextInfo> options) : base(options) { }
        
           public DbSet<UserContext> DataBase { get; set; }
        public DbSet<ShopInfo> Upgrades { get; set; }
        public DbSet<UserInfo> UserUpgrades { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserInfo>().HasKey(x => new { x.UserLogin, x.UpgradeId });
            modelBuilder.Entity<UserInfo>().HasOne(x => x.User).WithMany(u => u.UserUpgrades).HasForeignKey(x => x.UserLogin);

            modelBuilder.Entity<UserInfo>().HasOne(x => x.Upgrade).WithMany().HasForeignKey(x => x.UpgradeId).OnDelete(DeleteBehavior.Cascade);
        }

    }
}
