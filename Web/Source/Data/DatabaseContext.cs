using Home.Source.Models;
using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Home.Source.Data
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Log> Logs { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Person>().ToTable("People");

            builder.Entity<User>(e =>
            {
                e.Property(p => p.FirstName).IsRequired(required: true).HasMaxLength(25);
                e.Property(p => p.LastName).IsRequired(required: true).HasMaxLength(25);
            });

            builder.Entity<Person>(e =>
            {
                e.Property(p => p.Id).HasColumnName("PersonId");

                e.Property(p => p.FirstName).IsRequired(required: true).HasMaxLength(25);
                e.Property(p => p.LastName).IsRequired(required: true).HasMaxLength(25);
            });


            builder.Entity<Log>(e =>
            {
                e.Property(p => p.Id).HasColumnName("LogId");

                e.Property(p => p.Comment).IsRequired(required: true).HasMaxLength(100);
            });

            builder.Entity<PersonIdResult>()
                .ToSqlQuery("EXEC sp_GetIds");
        }
    }
}
