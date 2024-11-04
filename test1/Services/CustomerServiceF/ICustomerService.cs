using test1.Dto;
using test1.Models;

namespace test1.Services.CustomerServiceF
{
    public interface ICustomerService
    {
        IEnumerable<CustomerReturn> GetCustomers();
        Customer GetCustomerById(int id);
        Customer CreateCustomer(CustomerDtoSignUp customer);
        object LoginCustomer(CustomerDto customer);
        object GetProfile(string email, List<string> roles);
        void DeleteCustomer(int id);
        Customer UpdateCustomer(int id, UpdateCustomerDto updatedCustomer);
        List<RoleDto> AddRoleToCustomer(AddRoleToCustomerDto model);
        bool CheckCustomer(string email);
        Customer GetCustomerByEmail(string email);
        //List<CustomerRole> GetCustomerRoleById(int id);
        //List<Role> GetRoles(List<int> roleIds);
    }
}
