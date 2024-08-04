using test1.Dto;
using test1.Models;

namespace test1.Interfaces
{
    public interface ICustomerRepository
    {
        ICollection<Customer> GetCustomers();
        Customer GetCustomerById(int id);

        Customer GetCustomerByName(string name);
        Customer GetCustomerByEmail(string email);

        bool CustomerExists(int id);

        bool CustomerExistsByEmail(string email);
        Customer CustomerExistsByName(string name);

        Customer CreateCustomer(CustomerDto customer);
        bool Save();   

        Customer LoginCustomer(Customer customer);
        Customer Profile(Customer customer);
        void DeleteCustomer(Customer customer);
        void UpdateCustomer(Customer customer); 
    }
}
