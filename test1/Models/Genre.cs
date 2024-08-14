using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test1.Models
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int GenreId { get; set; }

        [Required]
        [MaxLength(30)]
        public string GenreName { get; set;}

        [Required]
        [MaxLength(255)]
        public string Description { get; set;}

        public virtual ICollection<Movie> Movie { get; set; }
    }
}
