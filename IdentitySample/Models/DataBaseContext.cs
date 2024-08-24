using IdentitySample.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentitySample.Models
{
    public class DataBaseContext : IdentityDbContext<User, Role, string>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.ProviderKey, p.LoginProvider });
            builder.Entity<IdentityUserRole<string>>().HasKey(p => new {p.UserId, p.RoleId});
            builder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });

            builder.Entity<User>().Ignore(p => p.NormalizedEmail);
        }
    }
}
