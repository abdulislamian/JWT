using JWTAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Data
{
    public class JWTDbContext : IdentityDbContext<ApplicationUser>
    {
        public JWTDbContext(DbContextOptions<JWTDbContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<TokenStore> tokenStores { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var AdminId = "7eb3cc43-4661-47c7-ab04-c4e2b2693e56";
            var UserId = "bd1c7750-7cf5-4a51-9e0e-35181826d11d";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id =AdminId,
                    ConcurrencyStamp=AdminId,
                    Name ="Admin",
                    NormalizedName = "Admin".ToUpper()
                },
                new IdentityRole()
                {
                    Id =UserId,
                    ConcurrencyStamp=UserId,
                    Name ="User",
                    NormalizedName = "User".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
