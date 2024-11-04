using test1.Dto;
using test1.Models;

namespace test1.Services.MovieServiceF
{
    public interface IMovieService
    {
        IEnumerable<MovieDto> GetMovies();
        
        MovieDto GetMovieById(int id);
        MovieDto GetMovieByTitle(string title);
        Movie CreateMovie(MovieDto movie, string email);
        void DeleteMovie(string title);
        Movie UpdateMovie(MovieDto movie);
        bool CheckMovie(string title);
        List<Movie> GetMoviesByCustomer(string email);
        
    }
}
