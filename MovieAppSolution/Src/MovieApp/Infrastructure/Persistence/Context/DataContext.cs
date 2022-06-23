using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Entities;

namespace MovieApp.Infrastructure.Persistence.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "User");
            });

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(p => new { p.UserId, p.RoleId });

            //modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            //{
            //    entity.ToTable("UserClaims");
            //});

            //modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            //{
            //    entity.ToTable("UserLogins");
            //});

            //modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            //{
            //    entity.ToTable("RoleClaims");

            //});

            //modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            //{
            //    entity.ToTable("UserTokens");
            //});
        }
    }
}
