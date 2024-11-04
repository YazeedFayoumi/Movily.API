using AutoMapper;
using Nest;
using System.Security.Claims;
using test1.Dto;
using test1.Interfaces;
using test1.Models;
using test1.Services.CustomerServiceF;

namespace test1.Services.MovieServiceF
{
    public class MovieService : IMovieService
    {
        private readonly IRepo<Movie> _repository;
        private readonly IRepo<Genre> _genre;
        private readonly IRepo<Customer> _customer;

        private readonly ICustomerService _customerService;
        public IMapper _mapper;

       
        public MovieService(IRepo<Movie> repo, IRepo<Genre> genre, IMapper mapper, IRepo<Customer> customer)
        {
            _repository = repo;
            _genre = genre;
            _mapper = mapper;
            _customer = customer;
        }
        public bool CheckMovie(string title)
        {
           bool alreadyExists = _repository.Exists(t => t.Title == title);
           return alreadyExists;
        }

        public Movie CreateMovie(MovieDto movie, string email)
        {
            Movie addedMovie = new Movie
            {
                Title = movie.Title,
                Description = movie.Description,
                Rating = 0,
                Duration = movie.Duration,
                AddedByUser = email
            };

           _repository.Create(addedMovie);
           return addedMovie;
        }

        public void DeleteMovie(string title)
        {
            Movie movie = _repository.GetByCondition(t => t.Title == title);

            _repository.Delete(movie);
            
        }

        public MovieDto GetMovieById(int id)
        {
            var movie = _repository.GetByCondition(i => i.MovieId == id);


            var movieDto = _mapper.Map<MovieDto>(movie);


            return (movieDto);
        }

        public MovieDto GetMovieByTitle(string title)
        {

            var movie = _repository.GetByCondition(i => i.Title == title);


            var movieDto = _mapper.Map<MovieDto>(movie);


            return (movieDto);
        }

        public IEnumerable<MovieDto> GetMovies()
        {
            var movies = _repository.GetAll();
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);

            return (movieDtos);
        }

        public List<Movie> GetMoviesByCustomer(string email)
        {
            Customer customer = _customer.GetByCondition(e => e.Email == email);
            var movies = _repository.GetListByCondition(


                m => m.AddedByUser == customer.Email
               
            );
            //var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);
            return (movies);
        }

        public Movie UpdateMovie(MovieDto movie)
        {

            var existingMovie = _repository.GetByCondition(t => t.Title == movie.Title);
           
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

            Movie updatedMovie = _repository.GetByCondition(t => t.Title == existingMovie.Title);

            return (updatedMovie); 
        }
        

    }
}
