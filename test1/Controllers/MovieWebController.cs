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
    public class MovieWebController : ControllerBase
    {
        public IMovieRepository _movieRepository;
        private readonly ICustomerRepository _customerRepository;
        public IMapper _mapper;

        public MovieWebController(IMovieRepository movieRepository, IMapper mapper, ICustomerRepository customerRepository)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _customerRepository = customerRepository;
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
            if (movie == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_movieRepository.CheckMovieByTitle(movie.Title))
            {
                ModelState.AddModelError("", "Movie Already exists, if both share the same name add the release year with the name");
                return StatusCode(422, ModelState);
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            Customer customerAdded = _customerRepository.GetCustomerByEmail(userEmail);
            Movie addedMovie = _movieRepository.AddMovie(movie, userEmail, customerAdded);

            return Ok(addedMovie);
        }
        [AuthorizeRole(Roles = "Support")]
        [HttpGet("AllMovies")]
        public IActionResult GetAllMovies()
        {
            var movies = _movieRepository.GetAllMovies();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);

            return Ok(movieDtos);
        }

        [HttpGet("GetMovieByTitle")]
        [ProducesResponseType(200, Type = typeof(MovieDto))]
        [ProducesResponseType(404)]
        public IActionResult GetMovieById(string title)
        {
            var movie = _movieRepository.GetMovieByTitle(title);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDto>(movie);


            return Ok(movieDto);
        }
        [HttpGet("GetMoviesByCustomer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetMoviesByCustomer(string email)
        {
            if (!_customerRepository.CustomerExistsByEmail(email))
            {
                return NotFound();
            }

            var movies = _movieRepository.GetMoviesByCustomerEmail(email);
            var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);
            return Ok(movieDtos);
        }
        [HttpPut("EditMovie")]
        public ActionResult EditMovie([FromBody] MovieDto movie)
        {
            if (movie == null)
            {
                return BadRequest("Invalid data.");
            }

            var existingMovie = _movieRepository.GetMovieByTitle(movie.Title);
            if (existingMovie == null)
            {
                return NotFound("Movie not found.");
            }

            Movie updatedMovie = _movieRepository.EditMovie(movie, existingMovie);
           

            
           

            return Ok(updatedMovie);
        }
        [HttpDelete("DeleteMovie/{title}"), Authorize]
        public IActionResult DeleteMovie([FromRoute] string title)
        {
            if (!_movieRepository.CheckMovieByTitle(title))
            {
                return NotFound("Movie not found.");
            }

            Movie movie = _movieRepository.GetMovieByTitle(title);
            if (movie == null)
            {
                return NotFound("Customer not found.");
            }

            bool movieDeleted = _movieRepository.DeleteMovieByTitle(movie);
            return Ok(movieDeleted);

        }
    }
}
