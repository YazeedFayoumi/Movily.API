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
    public class MovieController : ControllerBase
    {
        /*private readonly IRepo<Movie> _repository;
        private readonly IRepo<Genre> _genreRepository;
        private  readonly IRepo<Customer> _customerRepository;*/
        public IMapper _mapper;
        private readonly IMovieService _movieService;

        public MovieController(IMapper mapper, IMovieService movieService)
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
            /*if (movie == null || !ModelState.IsValid)
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

            return Ok(addedMovie);*/
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
           /* var movies = _repository.GetAll();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);

            return Ok(movieDtos);*/
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
            /*var movie = _repository.GetByCondition( t => t.Title == title);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDto>(movie);


            return Ok(movieDto);*/
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
            /*if (!_customerRepository.Exists(e => e.Email == email))
            {
                return NotFound();
            }
            var customer = _customerRepository.GetByCondition(e => e.Email == email);
            var movies = _repository.GetListByCondition(


                m => m.AddedByUser == customer.Email
               
            );
            //var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);
            return Ok(movies);*/
        
            var moives = _movieService.GetMoviesByCustomer(email);
            return Ok(moives);
        }

        [HttpPut("EditMovie")]
        public ActionResult EditMovie([FromBody] MovieDto movie)
        {
            /*if (movie == null)
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
            return Ok(updatedMovie);*/
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
            /*if (!_repository.Exists(t => t.Title == title))
            {
                return NotFound("Movie not found.");
            }

            Movie movie = _repository.GetByCondition(t => t.Title == title);
            if (movie == null)
            {
                return NotFound("Customer not found.");
            }

            _repository.Delete(movie);
            return Ok();*/
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
            /*var movie = _repository.GetByCondition( t => t.Title == title);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDto>(movie);


            return Ok(movieDto);*/
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
            {
                return BadRequest();
            }
            return Ok(movie);
        }
    }
}
