using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using test1.Models;
namespace test1.Configration
{
    public class CustomerRoleConfig : IEntityTypeConfiguration<CustomerRole>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CustomerRole> builder)
        {
            // Configure the composite primary key
            builder.HasKey(cr => new { cr.CustomerId, cr.RoleId });

            // Configure the relationship between Customer and CustomerRole
            builder.HasOne(cr => cr.Customer)
                   .WithMany(c => c.CustomerRoles)
                   .HasForeignKey(cr => cr.CustomerId);

            // Configure the relationship between Role and CustomerRole
            builder.HasOne(cr => cr.Role)
                   .WithMany(r => r.CustomerRoles)
                   .HasForeignKey(cr => cr.RoleId);

            // Map the entity to an existing table if needed
            builder.ToTable("CustomerRoles"); // Ensure this matches the table name
        }
    }
}
