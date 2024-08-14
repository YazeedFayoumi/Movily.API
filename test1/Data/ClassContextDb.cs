using Microsoft.EntityFrameworkCore;
using test1.Models;


namespace test1.Data
{
    public class ClassContextDb : DbContext
    {
        public ClassContextDb(DbContextOptions<ClassContextDb> options) : base(options)
        {
        }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Genre> Genre { get; set; }

        public DbSet<MembershipType> MembershipType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
             .HasMany(c => c.Movie)
             .WithMany(m => m.Customer)
             .UsingEntity(j => j.ToTable("CustomerMovie"));
                


        }
    }
}

