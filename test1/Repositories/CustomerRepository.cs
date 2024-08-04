using System.Linq;
using test1.Data;
using test1.Dto;
using test1.Interfaces;
using test1.Models;

namespace test1.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ClassContextDb _context;
        public CustomerRepository(ClassContextDb context) 
        {
            _context = context;
        }

        public Customer CreateCustomer(CustomerDto customer)
        {

            var newCustomer = new Customer

            { 
                Email = customer.Email,
                Password = customer.Password,
                Name = string.Empty,
                MembershipTypeId = 0
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
    }
}
