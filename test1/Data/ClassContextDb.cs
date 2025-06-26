using Microsoft.EntityFrameworkCore;
using test1.Configration;
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
        public DbSet<Role> Role { get; set; }
        public DbSet<CustomerRole> CustomerRoles { get; set; }


        public DbSet<MembershipType> MembershipType { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
             .HasMany(c => c.Movie)
             .WithMany(m => m.Customer)
             .UsingEntity(j => j.ToTable("CustomerMovie"));

             modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin" },
                new Role { Id = 2, Name = "Admin" },
                new Role { Id = 3, Name = "User" },
                new Role { Id = 4, Name = "Support" });

            modelBuilder.ApplyConfiguration(new CustomerRoleConfig());
        }

    }
}

