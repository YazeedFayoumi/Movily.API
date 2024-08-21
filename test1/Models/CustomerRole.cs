namespace test1.Models
{
    public class CustomerRole
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
