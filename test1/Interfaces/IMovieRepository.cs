using test1.Controllers;
using test1.Dto;
using test1.Models;

namespace test1.Interfaces
{
    public interface IMovieRepository
    {
        Movie AddMovie(MovieDto movie, string email, Customer customerAdded); 
        bool CheckMovieByTitle(string title);
        ICollection<Movie> GetAllMovies();

        Movie GetMovieByTitle (string title);

        IEnumerable<Movie> GetMoviesByCustomerEmail(string email);

    }
}
