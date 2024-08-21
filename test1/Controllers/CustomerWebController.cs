using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using test1.Dto;
using test1.Interfaces;
using test1.Models;


namespace test1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CustomerWebController : ControllerBase
    {
        public ICustomerRepository _customerRepository;
        private readonly IConfiguration _configuration;
        public IMapper _mapper;

        //public static Customer customerObj = new Customer();

        public CustomerWebController(ICustomerRepository customerRepository, IConfiguration configuration, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("{GetString}")]
        //[Route("GetString")]
        public string GetString()
        {
            return "working";
        }
        [AuthorizeRole(Roles ="Admin")]
        [HttpGet("GetCustomers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
            return Ok(customerDtos);
        }
        [HttpGet("GetCustomerById/{id}")]
        public IActionResult GetCustomerById(int id)
        {
            if (!_customerRepository.CustomerExists(id))
            {
                return NotFound();
            }
            var customer = _customerRepository.GetCustomerById(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(customer);
        }
        [HttpPost("Register")] 
        public ActionResult<Customer> CreateCustomer([FromBody] CustomerDtoSignIn customer)
        {
            if (customer == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (_customerRepository.CustomerExistsByEmail(customer.Email))
            {
                ModelState.AddModelError("", "Customer already exists");
                return StatusCode(422, ModelState);
            }
            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(customer.Password);
            
            customer.Password = hashedPassword;
           
            var createdCustomer = _customerRepository.CreateCustomer(customer);

            return Ok(createdCustomer);
        }
        [HttpPost("Login")]
        public ActionResult LoginCustomer([FromBody] CustomerDto loginCustomer)
        {
            Customer customer = _customerRepository.GetCustomerByEmail(loginCustomer.Email);

            if (customer == null)
            {
                return BadRequest("User Was not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(loginCustomer.Password, customer.Password))
            {
                return BadRequest("Wrong password.");
            }
            
            string token = CreateToken(customer);
            
            
            string loggedInEmail = customer.Email;

            var response = new
            {
                Email = loggedInEmail,
                Token = token
            };

            return Ok(response);
            
            
        } 
        private string CreateToken(Customer customer)
        {
            List<CustomerRole> customerRole = _customerRepository.GetCustomerRoleById(customer.Id);

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, customer.Email),
                //new Claim(ClaimTypes.Role, customer.Roles)
                };
            List<int> roleIds = customerRole.Select(cr => cr.RoleId).ToList();
            List<Role> roles = _customerRepository.GetRoles(roleIds);
            foreach (var role in roles )
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name)); 
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:TokenKey").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        [HttpGet("Profile"), Authorize]
        public ActionResult GetProfile()
        {
            
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                return Unauthorized("No email claim found.");
            }

            var email = emailClaim.Value;

            
            var customer = _customerRepository.GetCustomerByEmail(email);
            if (customer == null)
            {
                return NotFound("Customer profile not found.");
            }

            
            var profileResponse = new
            {
                customer.Name,
                customer.Email,
                customer.MembershipTypeId,
                
            };

            return Ok(profileResponse);
        }

        [HttpDelete("DeleteCustomer/{id}"), Authorize]
        public IActionResult DeleteCustomer([FromRoute] int id)
        {
            if (!_customerRepository.CustomerExists(id))
            {
                return NotFound("Customer not found.");
            }

            var customer = _customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            _customerRepository.DeleteCustomer(customer);
            _customerRepository.Save();
            return NoContent();
        }
        [HttpPut("UpdateCustomer"), Authorize]
        public ActionResult UpdateCustomer([FromBody] Customer updatedCustomer)
        {
            if (updatedCustomer == null || updatedCustomer.Id <= 0)
            {
                return BadRequest("Invalid customer data.");
            }

            var existingCustomer = _customerRepository.GetCustomerById(updatedCustomer.Id);
            if (existingCustomer == null)
            {
                return NotFound("Customer not found.");
            }

            if (!string.IsNullOrEmpty(updatedCustomer.Password))
            {
                existingCustomer.Password = BCrypt.Net.BCrypt.HashPassword(updatedCustomer.Password);
            }

            existingCustomer.Name = updatedCustomer.Name;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.MembershipTypeId = updatedCustomer.MembershipTypeId;

            _customerRepository.UpdateCustomer(existingCustomer);
            _customerRepository.Save();

            return Ok(existingCustomer);
        }

        [HttpPatch("AddRole"), Authorize]
        public ActionResult<Customer> AddRoleToCustomer([FromBody] AddRoleToCustomerDto model)
        {
            Customer customer = _customerRepository.GetCustomerByEmail(model.CustomerEmail);

            if (! _customerRepository.CustomerExistsByEmail(customer.Email))
            {
                ModelState.AddModelError("", "The customer does not exist");
                return StatusCode(422, ModelState);
            }
            List<Role> roles = _customerRepository.AddRoleToCustomer(customer, model);
            List<RoleDto> roleDtos = roles.Select(role => new RoleDto
            {
                RoleName = role.Name
            }).ToList();


            return Ok(roleDtos);
        }
        /*[HttpPatch("EditCustomer"), Authorize]
        public ActionResult EditCustomer(int id, JsonPatchDocument<Customer> patchDoc )
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingCustomer = _customerRepository.GetCustomerById(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }
            
            patchDoc.ApplyTo(existingCustomer, ModelState);

            




            _customerRepository.EditCustomer(existingCustomer);
            _customerRepository.Save();

            return Ok(existingCustomer);
        }*/
    }
}
