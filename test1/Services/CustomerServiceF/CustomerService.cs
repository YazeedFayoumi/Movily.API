using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using test1.Dto;
using test1.Interfaces;
using test1.Models;

namespace test1.Services.CustomerServiceF
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepo<Customer> _repository;
        private readonly IRepo<Role> _roleRepository;
        private readonly IRepo<CustomerRole> _customerRoleRepo;
        public IMapper _mapper;
        IConfiguration _config;
        public CustomerService(IRepo<Customer> repo, IRepo<Role> roleRepo, IRepo<CustomerRole> crRepo, IMapper mapper, IConfiguration config) 
        {
            _repository = repo;
            _roleRepository = roleRepo;
            _customerRoleRepo = crRepo;
            _mapper = mapper;   
            _config = config;
        }
        public List<RoleDto> AddRoleToCustomer(AddRoleToCustomerDto model)
        {
            var customer = _repository.GetByCondition(e => e.Email == model.CustomerEmail);

           /* if (!_repository.Exists(c => c.Email == model.CustomerEmail))
            {
                ModelState.AddModelError("", "The customer does not exist");
                return StatusCode(422, ModelState);
            }*/

            List<Role> roles = _roleRepository.GetListByCondition(r => model.Roles.Contains(r.Name));
            foreach (var role in roles)
            {

                var customerRole = new CustomerRole
                {
                    CustomerId = customer.Id,
                    RoleId = role.Id
                };
                _customerRoleRepo.Add(customerRole);

            }

            _repository.Save();
            List<RoleDto> roleDtos = roles.Select(role => new RoleDto
            {
                RoleName = role.Name
            }).ToList();

            _repository.Update(customer);
            return (roleDtos);
        }

        public bool CheckCustomer(string emial)
        {
            if(_repository.Exists(e =>e.Email == emial))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Customer CreateCustomer(CustomerDtoSignUp customer)
        {
            /*if (_repository.Exists(c => c.Email == customer.Email))
            {
                ModelState.AddModelError("", "Customer already exists");
                return StatusCode(422, ModelState);
            }
*/
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(customer.Password);

            customer.Password = hashedPassword;
            Customer newCustomer = new Customer

            {
                Email = customer.Email,
                Password = customer.Password,
                Name = string.Empty,
                MembershipTypeId = customer.MembershipType
            };
            var createdCustomer = _repository.Create(newCustomer);

            return (createdCustomer);
        }

        public void DeleteCustomer(int id)
        {
            var customer = _repository.Get(id);

            _repository.Delete(customer);
            _repository.Save();
            
        }

        public Customer GetCustomerByEmail(string email)
        {
            Customer customer = _repository.GetByCondition(e => e.Email == email);
            
            return (customer);
        }

        public Customer GetCustomerById(int id)
        {
           /* if (_repository.Get(id)==null)
            {
                return NotFound();
            }*/
            Customer customer = _repository.Get(id);
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/
            return (customer);
        }

        public IEnumerable<CustomerReturn> GetCustomers()
        {
            var customers = _repository.GetAll();

            
            var customerDtos = _mapper.Map<List<CustomerReturn>>(customers);
            return(customerDtos);
        }
        
        public object GetProfile(string email, List<string> roles)
        {

            var customer = _repository.GetByCondition(e => e.Email == email);
            /*if (customer == null)
            {
                return NotFound("Customer profile not found.");
            }*/


            var profileResponse = new
            {
                customer.Name,
                customer.Email,
                customer.MembershipTypeId,
                customer.CustomerRoles.Count,
                Roles = roles

            };

            return (profileResponse);
        }

        public object LoginCustomer(CustomerDto loginCustomer)
        {
            Customer customer = _repository.GetByCondition(e => e.Email == loginCustomer.Email);

           /* if (customer == null)
            {
                return BadRequest("User Was not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(loginCustomer.Password, customer.Password))
            {
                return BadRequest("Wrong password.");
            }*/

            string token = CreateToken(customer);

            string loggedInEmail = customer.Email;

            var response = new
            {
                Email = loggedInEmail,
                Token = token
            };

            return (response);

             string CreateToken(Customer customer)
            {
                List<CustomerRole> GetCustomerRoleById(Customer customer)
                {
                    return _customerRoleRepo.GetListByCondition(c => c.CustomerId == customer.Id);
                }
                List<Role> GetRoles(List<int> roleIds)
                {
                    return _roleRepository.GetListByCondition(r => roleIds.Contains(r.Id));
                }

                List<CustomerRole> customerRole = GetCustomerRoleById(customer);

                List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, customer.Email),
                //new Claim(ClaimTypes.Role, customer.Roles)
                };
                List<int> roleIds = customerRole.Select(cr => cr.RoleId).ToList();
                List<Role> roles = GetRoles(roleIds);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _config.GetSection("Jwt:TokenKey").Value!));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                    );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
        }

        public Customer UpdateCustomer(int id, UpdateCustomerDto updatedCustomer)
        {
          /*  if (!_repository.Exists(i => i.Id == id))
            {
                return NotFound("Customer not found.");
            }*/

            var existingCustomer = _repository.Get(id);
           /* if (existingCustomer == null)
            {
                return NotFound("Customer not found.");
            }*/

            if (!string.IsNullOrEmpty(updatedCustomer.Password))
            {
                existingCustomer.Password = BCrypt.Net.BCrypt.HashPassword(updatedCustomer.Password);
            }

            existingCustomer.Name = updatedCustomer.Name;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.MembershipTypeId = updatedCustomer.MembershipTypeId;

            _repository.Update(existingCustomer);
            _repository.Save();

            return (existingCustomer);
        }

        
    }

}

