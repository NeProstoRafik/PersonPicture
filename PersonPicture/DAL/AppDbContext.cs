using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonPicture.Models;

namespace PersonPicture.DAL
{
    public class AppDbContext : IdentityDbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> DbPeoples { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
            .Metadata.FindNavigation(nameof(Person.Pictures))
            .SetPropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<Person>()

           .HasMany(e => e.Friends)
           .WithMany();

            modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasNoKey();

            modelBuilder.Entity<IdentityUserRole<string>>()
           .HasNoKey();

            modelBuilder.Entity<IdentityUserToken<string>>()
           .HasNoKey();
        }

    }
}


