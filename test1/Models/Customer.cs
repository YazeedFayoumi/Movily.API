using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test1.Models
{
    [Table("Customer")]
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Id { get; set; }

        [Required]
        [MaxLength(550)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MaxLength(550)]
        public string Email { get; set; }

        [Required]
        [MaxLength (550)]
        [MinLength (8)]
        public string Password { get; set; }

        //public virtual ICollection<Movie> Movie { get; set; }

        //public MembershipType MembershipType { get; set; }
        public int MembershipTypeId { get; set; }
    }

}
