using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using test1.Data;
using test1.Dto;
using test1.Interfaces;
using test1.Models;

namespace test1.Repositories
{
    public class Repository : ICustomerRepository , IMovieRepository , IGenreRepository
    {
        private readonly ClassContextDb _context;
        public Repository(ClassContextDb context) 
        {
            _context = context;
        }

        public Movie AddMovie(MovieDto movie, string email, Customer customerAdded)
        {
            Movie newMovie = new Movie

            {
                Title = movie.Title,
                Description = movie.Description,
                Duration = movie.Duration,
                ReleaseDate = movie.ReleaseDate,
                Rating = 0,
                AddedByUser = email
            };

            
            _context.Movie.Add(newMovie);
            
            newMovie.Customer.Add(customerAdded);
            Save();
            
            return newMovie;
        }

        public Customer CreateCustomer(CustomerDtoSignIn customer)
        {

            Customer newCustomer = new Customer

            { 
                Email = customer.Email,
                Password = customer.Password,
                Name = string.Empty,
                MembershipTypeId = customer.MembershipType
            };

            _context.Customer.Add(newCustomer);

            Save();
            return newCustomer;

        }


        public bool CustomerExists(int id)
        {
            return _context.Customer.Any(x => x.Id == id);
        }

        public bool CustomerExistsByEmail(string email)
        {
            return _context.Customer.Any(c => c.Email == email);
        }
        public Customer CustomerExistsByName(string name)
        {
            return _context.Customer.FirstOrDefault(c => c.Name == name);
        }

        public void DeleteCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
             _context.Customer.Remove(customer);

        }

        public void EditCustomer(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
        }

        public Customer GetCustomerByEmail(string email)
        {
            return _context.Customer
                       .Include(c => c.Roles)
                       .FirstOrDefault(c => c.Email == email);      // _context.Customer.FirstOrDefault(c => c.Email == email);

        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customer.Where(p => p.Id == id).FirstOrDefault();
        }

        public Customer GetCustomerByName(string name)
        {
            return _context.Customer.Where(p => p.Name == name).FirstOrDefault();
        }

        public ICollection<Customer> GetCustomers()
        {
            return _context.Customer.OrderBy(p => p.Id).ToList();
        }

        public bool CheckMovieByTitle(string title)
        {
            return _context.Movie.Any(t => t.Title == title);
        }

        public Customer LoginCustomer(Customer customer)
        {
            return (customer);
        }

        public Customer Profile(Customer customer)
        {
            return customer;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >0 ? true : false;
        }

        public void UpdateCustomer(Customer customer)
        {
            
        }

        public ICollection<Movie> GetAllMovies()
        {
            return _context.Movie.OrderBy(p => p.MovieId).ToList();
        }

        public Movie GetMovieByTitle(string title)
        {
            return _context.Movie.Where(p => p.Title == title).FirstOrDefault();
        }

        public IEnumerable<Movie> GetMoviesByCustomerEmail(string email)
        {
            return _context.Movie.Include(m => m.Customer).Where(m => m.AddedByUser == email).ToList();

        }

        public Genre AddGenre(GenreDto genre)
        {
            Genre newGenre = new Genre

            {
                GenreName = genre.GenreName,
                Description = genre.Description,
     
            };


            _context.Genre.Add(newGenre);
            /*Movie emptyMovie = new Movie();
            newGenre.Movie.Add(emptyMovie);*/
            
            Save();

            return newGenre;
        }

        public bool CheckGenreByName(string genreName)
        {
            return _context.Genre.Any(g => g.GenreName == genreName);
        }

        public Genre GetGenreByName(string genreName)
        {
            // return _context.Genre.Where(p => p.GenreName == genreName).FirstOrDefault();
            return _context.Genre
         .Include(g => g.Movie)
         .FirstOrDefault(g => g.GenreName == genreName);
        }

        public Genre AddMovieToGenre(Movie movie, Genre genre)
        {
            if (genre.Movie == null)
            {
                genre.Movie = new List<Movie>();
            }
            genre.Movie.Add(movie);
            Save();
            return genre;
        }

        public bool DeleteMovieByTitle(Movie movie)
        {
           bool wasDeleted;
           string movieTitle = movie.Title;
            _context.Movie.Remove(movie);
            Save();
            if (_context.Movie.Any(d => d.Title == movieTitle))
            {
                wasDeleted = false;
            }
            else
            {
                wasDeleted= true;
            }
            //Save();
            return wasDeleted;
        }

        public Movie EditMovie(MovieDto movie, Movie existingMovie)
        {
            //Movie editedMovie = new Movie();
            if (movie.Title != existingMovie.Title)
            {
                existingMovie.Title = movie.Title;
            }
           // else { editedMovie.Title = existingMovie.Title; }
            if(movie.Description != existingMovie.Description)
            {
                existingMovie.Description = movie.Description;
            }
            //else { editedMovie.Description = editedMovie.Description; }

            if (movie.ReleaseDate != existingMovie.ReleaseDate)
            {
                existingMovie.ReleaseDate = movie.ReleaseDate;
            }
           // else { editedMovie.ReleaseDate=existingMovie.ReleaseDate; }
            if (movie.Duration != existingMovie.Duration)
            {
                existingMovie.Duration = movie.Duration;
            }
           // else
            {
                existingMovie.Duration = existingMovie.Duration;
            }
            if (movie.Rating != existingMovie.Rating)
            {
                existingMovie.Rating = movie.Rating;
            }
            // else { editedMovie.Rating = existingMovie.Rating; }
            existingMovie.AddedByUser = existingMovie.AddedByUser;
            _context.Movie.Update(existingMovie);
            
            Save();
            return existingMovie;
        }

        public ICollection<Genre> GetAllGenres()
        {
            return _context.Genre.OrderBy(p => p.GenreId).ToList();
        }

        public ICollection<Movie> GetGenreMovies(Genre genre)
        {
            if (genre.Movie == null)
            {
                return new List<Movie>();
            }

            return genre.Movie.ToList();

        }

        public bool DeleteGenreByName(Genre genre)
        {
            bool wasDeleted;
            string genreName = genre.GenreName;
            _context.Genre.Remove(genre);
            Save();
            if (_context.Genre.Any(d => d.GenreName == genreName))
            {
                wasDeleted = false;
            }
            else
            {
                wasDeleted= true;
            }
           
            return wasDeleted;
        }

        public List<Role> AddRoleToCustomer(Customer customer, AddRoleToCustomerDto model)
        {
            /* if (customer == null)
             {
                 throw new Exception("Customer not found");
             }
             var roles = _context.Role.Where(r => model.Roles.Contains(r.Name)).ToList();

             foreach (var role in roles)
             {
                 if (!customer.Roles.Contains(role))
                 {
                     customer.Roles.Add(role);
                     _context.CustomerRoles.Add(new CustomerRole
                     {
                         CustomerId = customer.Id,
                         RoleId = role.Id
                     });
                 }
             }

             Save();
             List<Role> cusotmerRoles =  _context.CustomerRoles
         .Where(cr => cr.CustomerId == customer.Id)
         .Select(cr => cr.Role)
         .ToList(); //customer.Roles.ToList();   
             return cusotmerRoles;*/
            var roles = _context.Role.Where(r => model.Roles.Contains(r.Name)).ToList();

            // Retrieve existing roles of the customer
            

            foreach (var role in roles)
            {
                // Check if the customer already has this role
               // customer.Roles.Add(role);
                
                    // Add the new role to the customer's roles
                    _context.CustomerRoles.Add(new CustomerRole
                    {
                        CustomerId = customer.Id,
                        RoleId = role.Id
                    });
                
            }

            // Save changes to the database
            Save();

            // Return the updated list of roles for the customer
            var updatedRoles = _context.CustomerRoles
                .Where(cr => cr.CustomerId == customer.Id)
                .Select(cr => cr.Role)
                .ToList();

            return updatedRoles;

        }
           

     

        public List<Role> GetRoles(List<int> roleIds)
        {
            return _context.Role
           .Where(r => roleIds.Contains(r.Id))
           .ToList();
        }

        public List<CustomerRole> GetCustomerRoleById(int id)
        {
            Customer customer = GetCustomerById(id);

            List<CustomerRole> customerRole = _context.CustomerRoles.Where(c => c.CustomerId == customer.Id).ToList();
            
            //_context.CustomerRoles.FirstOrDefault()
            return customerRole;
        }
    }
}
