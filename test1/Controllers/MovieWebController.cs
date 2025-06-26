using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using test1.Dto;
using test1.Interfaces;
using test1.Models;
using test1.Services.CustomerServiceF;
using test1.Services.GenreServiceF;
using test1.Services.MovieServiceF;

namespace test1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieWebController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IMovieService _movieService;

        public MovieWebController(IMapper mapper, IMovieService movieService)
        {
            _mapper = mapper;
            _movieService = movieService;
        }

        [AuthorizeRole(Roles = "Admin")]
        [HttpGet("{GetString}")]
        public string GetString()
        {
            return "working!";
        }

        [HttpPost("AddMovie"), Authorize]
        public ActionResult<Movie> AddMovie([FromBody] MovieDto movie)
        {
            if(_movieService.CheckMovie(movie.Title))
            {
                ModelState.AddModelError("", "Movie Already exists, if both share the same name add the release year with the name");
                return StatusCode(422, ModelState);
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var addedMovie = _movieService.CreateMovie(movie, userEmail);
            return Ok(addedMovie);
        }

        [AuthorizeRole(Roles = "Support")]
        [HttpGet("AllMovies")]
        public IActionResult GetAllMovies()
        {
           var allMovies = _movieService.GetMovies();
           if(allMovies == null)
            {
                return BadRequest();
            }
           return Ok(allMovies);
        }

        [HttpGet("GetMovieByTitle")]
        [ProducesResponseType(200, Type = typeof(MovieDto))]
        [ProducesResponseType(404)]
        public IActionResult GetMovieByTitle(string title)
        {
            var movie = _movieService.GetMovieByTitle(title);
            if(movie == null)
            {
                return BadRequest();
            }
            return Ok(movie);
        }

        [HttpGet("GetMoviesByCustomer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetMoviesByCustomer(string email)
        {
            var moives = _movieService.GetMoviesByCustomer(email);
            return Ok(moives);
        }

        [HttpPut("EditMovie")]
        public ActionResult EditMovie([FromBody] MovieDto movie)
        {
            if (movie == null)
            {
                return BadRequest("Invalid data.");
            }
            Movie updatedMovie = _movieService.UpdateMovie(movie);
            return Ok(updatedMovie);
        }

        [HttpDelete("DeleteMovie/{title}"), Authorize]
        public IActionResult DeleteMovie([FromRoute] string title)
        {
            if (!_movieService.CheckMovie(title))
            {
                return NotFound("Movie not found.");
            }
            _movieService.DeleteMovie(title);
            return Ok();
        }

        [HttpGet("MovieById/{id}")]
        [ProducesResponseType(200, Type = typeof(MovieDto))]
        [ProducesResponseType(404)]
        public IActionResult GetMovieById(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
            {
                return BadRequest();
            }
            return Ok(movie);
        }
    }
}
