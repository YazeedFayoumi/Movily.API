using System.ComponentModel.DataAnnotations;
using test1.Models;

namespace test1.Dto
{
    public class UpdateCustomerDto
    {
        public string Name { get; set; }

       
        public string Email { get; set; }

        
        public string Password { get; set; }

        public int MembershipTypeId { get; set; }

    }
}
