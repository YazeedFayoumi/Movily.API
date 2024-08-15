using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using test1.Dto;
using test1.Interfaces;
using test1.Models;

namespace test1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreWebController : ControllerBase
    {
        public IGenreRepository _genreRepository;
        public IMovieRepository _movieRepository;
        public IMapper _mapper;

        public GenreWebController(IMovieRepository movieRepository, IMapper mapper, IGenreRepository genre)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _genreRepository = genre;
        }
        [HttpPost("AddGenre"), Authorize]
        public ActionResult<Genre> AddGenre([FromBody] GenreDto genre)
        {
            if (genre == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_genreRepository.CheckGenreByName(genre.GenreName))
            {
                ModelState.AddModelError("", "This genre was already added.");
                return StatusCode(422, ModelState);
            }
            
           
            Genre addedGenre = _genreRepository.AddGenre(genre);
            return Ok(addedGenre);
        }
        [AuthorizeRole(Roles ="User")]
        [HttpPatch("AddMovieToGenre"), Authorize]
        public ActionResult<Genre> AddMovieToGenre([FromBody] MovieToGenreDto model)
        {
            if (model.Title == null  && ModelState.IsValid && model.GenreName == null)
            {
                return BadRequest(ModelState);
            }

            if (!_genreRepository.CheckGenreByName(model.GenreName))
            {
                ModelState.AddModelError("", "The genre does not exist");
                return StatusCode(422, ModelState);
            }

            Movie movie = _movieRepository.GetMovieByTitle(model.Title);
            Genre genre = _genreRepository.GetGenreByName(model.GenreName);
            Genre newGenre = _genreRepository.AddMovieToGenre(movie, genre);
            return Ok(newGenre);
        }
        [HttpGet("GetAllGenres")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public IActionResult GetCustomers()
        {
            var genres = _genreRepository.GetAllGenres();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var genreDtos = _mapper.Map<List<GenreDto>>(genres);
            return Ok(genreDtos);
        }
        [HttpGet("GetGenreMovies/{genreName}")]
        public IActionResult GetGenreMovies(string genreName)
        {
            var genre = _genreRepository.GetGenreByName(genreName);
            if(genre == null)
            {
                return BadRequest(ModelState);
            }
            var genreMovies = _genreRepository.GetGenreMovies(genre);
            var genreMoviesDtos = _mapper.Map<List<MovieDto>>(genreMovies);
            return Ok(genreMoviesDtos);
        }
        [AuthorizeRole(Roles ="Support")]
        [HttpDelete("DeleteGenre/{genreName}"), Authorize]
        public IActionResult DeleteGenre([FromRoute] string genreName)
        {
            if (!_genreRepository.CheckGenreByName(genreName))
            {
                return NotFound("Genre not found.");
            }

            Genre genre = _genreRepository.GetGenreByName(genreName);
            if (genre == null)
            {
                return NotFound("Genre not found.");
            }

            bool genreDeleted = _genreRepository.DeleteGenreByName(genre);
            return Ok(genreDeleted);

        }
    }
}
