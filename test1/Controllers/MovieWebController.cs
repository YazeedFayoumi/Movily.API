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
        private readonly IRepo<Movie> _repository;
        private readonly IRepo<Genre> _genreRepository;
        private  readonly IRepo<Customer> _customerRepository;
        public IMapper _mapper;

        public MovieWebController(IRepo<Movie> repository, IMapper mapper, IRepo<Genre> genreRepository, IRepo<Customer> customerRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _genreRepository = genreRepository;
            _customerRepository=customerRepository;
        }
        [AuthorizeRole(Roles = "Admin")]
        [HttpGet("{GetString}")]
        public string GetString()
        {
            return "working!";
        }

        [HttpPost("AddMovie")]
        public ActionResult<Movie> AddMovie([FromBody] MovieDto movie)
        {
            if (movie == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_repository.Exists(m => m.Title == movie.Title))
            {
                ModelState.AddModelError("", "Movie Already exists, if both share the same name add the release year with the name");
                return StatusCode(422, ModelState);
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            Customer customerAdded = _customerRepository.GetByCondition(e => e.Email == userEmail);
            Movie addedMovie = new Movie
            { 
                Title = movie.Title,
                Description = movie.Description,
                Rating = 0,
                Duration = movie.Duration
            
            };

            
            addedMovie.AddedByUser = userEmail;
            Movie newMovie = _repository.Create(addedMovie);

            return Ok(addedMovie);
        }
        [AuthorizeRole(Roles = "Support")]
        [HttpGet("AllMovies")]
        public IActionResult GetAllMovies()
        {
            var movies = _repository.GetAll();

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
            var movie = _repository.GetByCondition( t => t.Title == title);

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
            if (!_customerRepository.Exists(e => e.Email == email))
            {
                return NotFound();
            }
            var customer = _customerRepository.GetByCondition(e => e.Email == email);
            var movies = _repository.GetListByCondition(


                m => m.AddedByUser == customer.Email
               
            );
            //var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);
            return Ok(movies);
        }
        [HttpPut("EditMovie")]
        public ActionResult EditMovie([FromBody] MovieDto movie)
        {
            if (movie == null)
            {
                return BadRequest("Invalid data.");
            }

            var existingMovie = _repository.GetByCondition(t => t.Title == movie.Title);
            if (existingMovie == null)
            {
                return NotFound("Movie not found.");
            }
            if (movie.Title != existingMovie.Title)
            {
                existingMovie.Title = movie.Title;
            }
            
            if (movie.Description != existingMovie.Description)
            {
                existingMovie.Description = movie.Description;
            }
            

            if (movie.ReleaseDate != existingMovie.ReleaseDate)
            {
                existingMovie.ReleaseDate = movie.ReleaseDate;
            }
            
            if (movie.Duration != existingMovie.Duration)
            {
                existingMovie.Duration = movie.Duration;
            }
           
            {
                existingMovie.Duration = existingMovie.Duration;
            }
            if (movie.Rating != existingMovie.Rating)
            {
                existingMovie.Rating = movie.Rating;
            }
            
            existingMovie.AddedByUser = existingMovie.AddedByUser;

            _repository.Update(existingMovie);

            Movie updatedMovie = _repository.GetByCondition(t =>t.Title == existingMovie.Title);    




            return Ok(updatedMovie);
        }
        [HttpDelete("DeleteMovie/{title}"), Authorize]
        public IActionResult DeleteMovie([FromRoute] string title)
        {
            if (!_repository.Exists(t => t.Title == title))
            {
                return NotFound("Movie not found.");
            }

            Movie movie = _repository.GetByCondition(t => t.Title == title);
            if (movie == null)
            {
                return NotFound("Customer not found.");
            }

            _repository.Delete(movie);
            return Ok();

        }
    }
}
