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
          _genreService = genreService;
        }
        [HttpGet("AllGenres")]
        public IActionResult GetGenres()
        {
           var genres = _genreService.GetGenres();
            return Ok(genres);
        }
        [HttpGet("Genre")]
        public IActionResult GetGenre(int id)
        {
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
            Genre addedGenre = _genreService.CreateGenre(genre);
            return Ok(addedGenre);
        }

        [HttpPatch("AddMovieToGenre")]
        public ActionResult<Genre> AddMovieToGenre([FromBody] MovieToGenreDto model)
        {
            Genre newGenre = _genreService.AddMovieToGenre(model);
            return Ok(newGenre);
        }

        [HttpGet("GetGenreMovies/{genreName}")]
        public IActionResult GetGenreMovies(string genreName)
        {
            var genreMoviesDtos = _genreService.GetMoviesByGenre(genreName);
            return Ok(genreMoviesDtos);
        }


        [AuthorizeRole(Roles = "Support")]
        [HttpDelete("DeleteGenre/{genreName}"), Authorize]
        public IActionResult DeleteGenre([FromRoute] string genreName)
        {
            _genreService.DeleteGenre(genreName);
            return Ok();
        }
    }
}
