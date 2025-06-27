using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Security.Claims;
using test1.Dto;
using test1.Interfaces;
using test1.Models;
using test1.Repositories;
using test1.Services.GenreServiceF;

namespace test1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreWebController : ControllerBase
    {
        
        private readonly IGenreService _genreService;

        public GenreWebController(IGenreService genreService)
        {
          /*  _mapper = mapper;
            _repository = repository;
            _movieRepository = movieRepository;*/
          _genreService = genreService;
        }
        [HttpGet("AllGenres")]
        public IActionResult GetGenres()
        {
           /* var genres = _repository.GetAll();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var genreDtos = _mapper.Map<List<GenreDto>>(genres);
            return Ok(genreDtos);*/
           var genres = _genreService.GetGenres();
            return Ok(genres);
        }
        [HttpGet("Genre")]
        public IActionResult GetGenre(int id)
        {
            /*Genre genre = _repository.Get(id);
            if (!ModelState.IsValid || genre == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(genre);*/
            var genre = _genreService.GetGenreById(id);
            if (!ModelState.IsValid || genre == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(genre);
        }
        [HttpPost("AddGenre"), Authorize]
        public ActionResult<Genre> AddGenre([FromBody] GenreDto genre)
        {
            /* if (genre == null || !ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             if (_repository.Exists(g => g.GenreName == genre.GenreName))
             {
                 ModelState.AddModelError("", "This genre was already added.");
                 return StatusCode(422, ModelState);
             }

             Genre newGenre = new Genre

             {
                 GenreName = genre.GenreName,
                 Description = genre.Description,

             };
             Genre addedGenre = _repository.Create(newGenre);
             return Ok(addedGenre);*/
            Genre addedGenre = _genreService.CreateGenre(genre);
            return Ok(addedGenre);
        }

        /* [AuthorizeRole(Roles ="User")]
         [HttpPatch("AddMovieToGenre"), Authorize]
         public ActionResult<Genre> AddMovieToGenre([FromBody] MovieToGenreDto model)
         {
             if (model.Title == null  && ModelState.IsValid && model.GenreName == null)
             {
                 return BadRequest(ModelState);
             }

             if (!_repository.Exists(n => n.GenreName == model.GenreName))
             {
                 ModelState.AddModelError("", "The genre does not exist");
                 return StatusCode(422, ModelState);
             }
             Genre genre = _repository.GetByCondition<Genre>(n => n.GenreName == model.GenreName);
             Movie movie = _repository.GetByCondition<Movie>(m => m.Title == model.Title);
             genre.Movie = (ICollection<Movie>)movie;
             Genre newGenre = genre;
             _repository.Add(newGenre);
             return Ok(newGenre);
         }*/
        [HttpPatch("AddMovieToGenre")]
        public ActionResult<Genre> AddMovieToGenre([FromBody] MovieToGenreDto model)
        {
           /* if (model.Title == null || model.GenreName == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.Exists(n => n.GenreName == model.GenreName))
            {
                ModelState.AddModelError("", "The genre does not exist");
                return StatusCode(422, ModelState);
            }

            // Retrieve the genre and the movie based on the provided names
            Genre genre = _repository.GetByCondition(n => n.GenreName == model.GenreName);
            Movie movie = _movieRepository.GetByCondition(m => m.Title == model.Title);

           *//* if (movie == null)
            {
                ModelState.AddModelError("", "The movie does not exist");
                return StatusCode(422, ModelState);
            }
*//*
            if (genre.Movie == null)
            {
                genre.Movie = new List<Movie>();
            }
           // genre.Movie.Add(movie);

            _repository.Update(genre);
            _repository.Save();

            return Ok(genre);*/
            Genre newGenre = _genreService.AddMovieToGenre(model);
            return Ok(newGenre);
        }

        /* [HttpGet("GetGenreMovies/{genreName}")]
         public IActionResult GetGenreMovies(string genreName)
         {
             // Genre genre = _repository.GetByCondition<Genre>(g => g.GenreName == genreName );
             Genre genre = _repository.GetListByCondition<Genre>(
          g => g.GenreName == genreName,
          t => t.Movie // Specify the related entity to be eagerly loaded
      );
             if (genre == null)
             {
                 return BadRequest(ModelState);
             }
             //var genreMovies = _repository.GetListByCondition<Movie>(g => g.Genre == genre );
             //List<Movie> genreMovies = genre.Movie.ToList();
             var genreMoviesDtos = _mapper.Map<List<MovieDto>>(genreMovies);
             return Ok(genreMovies);
         }*/
        [HttpGet("GetGenreMovies/{genreName}")]
        public IActionResult GetGenreMovies(string genreName)
        {
            /*var genre = _repository.GetByCondition(
                g => g.GenreName == genreName,
                g => g.Movie
            );

            if (genre == null)
            {
                return NotFound("The specified genre does not exist.");
            }

            var genreMoviesDtos = _mapper.Map<List<MovieDto>>(genre.Movie.ToList());

            return Ok(genreMoviesDtos);*/
            var genreMoviesDtos = _genreService.GetMoviesByGenre(genreName);

            return Ok(genreMoviesDtos);

        }


        [AuthorizeRole(Roles = "Support")]
        [HttpDelete("DeleteGenre/{genreName}"), Authorize]
        public IActionResult DeleteGenre([FromRoute] string genreName)
        {
         /*   if (!_repository.Exists(g => g.GenreName == genreName))
            {
                return NotFound("Genre not found.");
            }

            Genre genre = _repository.GetByCondition(g => g.GenreName == genreName);
            if (genre == null)
            {
                return NotFound("Genre not found.");
            }

            try
            {
                _repository.Delete(genre);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Ok();*/
            _genreService.DeleteGenre(genreName);
            return Ok();
        }
    }
}
