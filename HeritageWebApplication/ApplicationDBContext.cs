using HeritageWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HeritageWebApplication
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole, int>
    {
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<HeritageObject> HeritageObjects { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<HeritageObject>()
                .HasOne(h => h.Building)
                .WithMany(o => o.HeritageObjects)
                .HasForeignKey(h => h.BuildingId);
            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.HeritageObject)
                .WithMany(h => h.Comments)
                .HasForeignKey(c => c.HeritageObjectId);
            
            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User)
                .WithMany(h => h.Comments)
                .HasForeignKey(u => u.UserId);
            
            modelBuilder.Entity<UserRole>().HasData(new UserRole()
            {
                Id = 1,
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            modelBuilder.Entity<UserRole>().HasData(new UserRole()
            {
                Id = 2,
                Name = "user",
                NormalizedName = "USER"
            });
            
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "heritage.app.admin@gmail.com",
                NormalizedEmail = "HERITAGE.APP.ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "admin123"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>()
            {
                RoleId = 1,
                UserId = 1
            });
        }
    }
}