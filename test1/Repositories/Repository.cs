using Microsoft.EntityFrameworkCore;
using System.Linq;
using test1.Data;
using test1.Dto;
using test1.Interfaces;
using test1.Models;

namespace test1.Repositories
{
    public class Repository : ICustomerRepository , IMovieRepository
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
            return _context.Customer.FirstOrDefault(c => c.Email == email);
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
    }
}
