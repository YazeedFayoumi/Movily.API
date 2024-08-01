using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test1.Models
{
    [Table("Movie")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(0, 10)]
        public float Rating { get; set; }

        [Required]
        [MaxLength(200)]
        [MinLength(15)]
        public string Description { get; set; }

        [Required]
        public int Duration { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Genre> Genre { get; set; }
    }
}
