using AutoMapper;
using test1.Dto;
using test1.Interfaces;
using test1.Models;
using test1.Services.CustomerServiceF;

namespace test1.Services.GenreServiceF
{
    public class GenreService : IGenreService
    {
        private readonly IRepo<Genre> _repository;
        private readonly IRepo<Movie> _movie;
        private readonly IRepo<Customer> _customer;
        public IMapper _mapper;

        public GenreService(IRepo<Genre> repo, IRepo<Movie> movie, IRepo<Customer> customer, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repo;
            _movie = movie;
            _customer = customer;
        }

        public Genre AddMovieToGenre(MovieToGenreDto model)
        {
           /* if (model.Title == null || model.GenreName == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.Exists(n => n.GenreName == model.GenreName))
            {
                ModelState.AddModelError("", "The genre does not exist");
                return StatusCode(422, ModelState);
            }*/

            // Retrieve the genre and the movie based on the provided names
            Genre genre = _repository.GetByCondition(n => n.GenreName == model.GenreName);
            Movie movie = _movie.GetByCondition(m => m.Title == model.Title);

           
            if (genre.Movie == null)
            {
                genre.Movie = new List<Movie>();
            }
           // genre.Movie.Add(movie);

            _repository.Update(genre);
            _repository.Save();

            return (genre);
        }

        public bool CheckMovie(string genreName)
        {
            throw new NotImplementedException();
        }

        public Genre CreateGenre(GenreDto genre)
        {
          /*  if (_repository.Exists(g => g.GenreName == genre.GenreName))
            {
                ModelState.AddModelError("", "This genre was already added.");
                return StatusCode(422, ModelState);
            }*/

            Genre newGenre = new Genre
            {
                GenreName = genre.GenreName,
                Description = genre.Description,
            };
            Genre addedGenre = _repository.Create(newGenre);
            return (addedGenre);
        }

        public void DeleteGenre(string genreName)
        {
       
            Genre genre = _repository.GetByCondition(g => g.GenreName == genreName);
            
            try
            {
                _repository.Delete(genre);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public GenreDto GetGenreById(int id)
        {
            Genre genre = _repository.Get(id);
            var genreDto = _mapper.Map<GenreDto>(genre);
            return(genreDto);
        }

        public IEnumerable<GenreDto> GetGenres()
        {
            var genres = _repository.GetAll();

            
            var genreDtos = _mapper.Map<List<GenreDto>>(genres);
            return (genreDtos);
        }

        public List<MovieDto> GetMoviesByGenre(string genreName)
        {
            var genre = _repository.GetByCondition(
                g => g.GenreName == genreName,
                g => g.Movie
            );

            var genreMoviesDtos = _mapper.Map<List<MovieDto>>(genre.Movie.ToList());

            return(genreMoviesDtos);
        }

        public Genre UpdateGenre(GenreDto genre)
        {
            throw new NotImplementedException();
        }
    }
}
