using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDBContext : IdentityDbContext
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRole = "55a303a1-5c3d-48c3-9dd8-b359b8fe7b17";
            var writerRole = "bec1eca3-c83d-4528-88fe-7419c2613a77";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRole,
                    Name = "Reader",
                    ConcurrencyStamp = readerRole,
                    NormalizedName  = "Reader".ToUpper()
                },
                new IdentityRole()
                {
                    Id = writerRole,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp  = writerRole
                }
            };

            builder.Entity<IdentityRole>().HasData(roles); // migration runs it seeds the data for the roles in the roles table 
            var adminUserId = "2879a699-49b6-4d5d-a799-98d29337f757";
            // create a admin user 
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Krishna");

            builder.Entity<IdentityUser>().HasData(admin);

            // give roles to admin user and seed the admin user 
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRole
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRole
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
    }
}
