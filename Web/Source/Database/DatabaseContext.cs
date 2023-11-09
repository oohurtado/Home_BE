using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Home.Source.DataBase
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(e =>
            {
                e.Property(p => p.FirstName).IsRequired(required: true).HasMaxLength(25);
                e.Property(p => p.LastName).IsRequired(required: true).HasMaxLength(25);
            });
        }
    }
}
