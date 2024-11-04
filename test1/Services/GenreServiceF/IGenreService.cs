using test1.Dto;
using test1.Models;

namespace test1.Services.GenreServiceF
{
    public interface IGenreService
    {
        IEnumerable<GenreDto> GetGenres();
        GenreDto GetGenreById(int id);
        //MovieDto GetMovieByTitle(string title);
        Genre CreateGenre(GenreDto genre);
        void DeleteGenre(string genreName);
        Genre UpdateGenre(GenreDto genre);
        bool CheckMovie(string genreName);
        List<MovieDto> GetMoviesByGenre(string genreName);
        Genre AddMovieToGenre(MovieToGenreDto model);
    }
}
