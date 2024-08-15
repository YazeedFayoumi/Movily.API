using test1.Dto;
using test1.Models;

namespace test1.Interfaces
{
    public interface IGenreRepository
    {
        Genre AddGenre(GenreDto genre);
        bool CheckGenreByName(string genreName);
        Genre GetGenreByName(string genreName);
        Genre AddMovieToGenre(Movie movie, Genre genre);
        ICollection<Genre> GetAllGenres();
        ICollection<Movie> GetGenreMovies(Genre genre);
        bool DeleteGenreByName(Genre genre);
    }
}
