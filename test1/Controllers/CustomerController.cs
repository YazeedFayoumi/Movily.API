using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using test1.Dto;
using test1.Models;
using test1.Services;

using test1.Services.CustomerServiceF;


namespace test1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CustomerController : ControllerBase
    {
        
      /*private readonly IRepo<Customer> _repository;
        private readonly IRepo<Role> _roleRepository;
        private readonly IRepo<CustomerRole> _customerRoleRepo;*/
        private readonly IConfiguration _configuration;
        public IMapper _mapper;
        private readonly ICustomerService _customerService;

        //public static Customer customerObj = new Customer();

        public CustomerController(IConfiguration configuration, IMapper mapper, ICustomerService customerService)
        {
           // _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
            _customerService = customerService;
            /*_roleRepository = roleRepo;
            _customerRoleRepo = crRole;*/
        }

        [HttpGet("{GetString}")]
        //[Route("GetString")]
        public string GetString()
        {
            return "working";
        }
        //[AuthorizeRole(Roles ="Admin")]
        [HttpGet("Customers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public ActionResult GetCustomers()
        {
            var customersDto = _customerService.GetCustomers();

            if (!ModelState.IsValid || customersDto == null)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(customersDto);
        }
        [HttpGet("ById/{id}")]
        public ActionResult GetCustomerById(int id)
        {
          
           var customer = _customerService.GetCustomerById(id);
           return Ok(customer);
        }

        [HttpPost("Register")]
        public ActionResult<Customer> CreateCustomer([FromBody] CustomerDtoSignUp customer)
        {
            /*if (customer == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (_repository.Exists(c => c.Email == customer.Email))
            {
                ModelState.AddModelError("", "Customer already exists");
                return StatusCode(422, ModelState);
            }

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

            return Ok(createdCustomer);*/
            if (customer == null)
            {
                return BadRequest();
            }
            if(_customerService.CheckCustomer(customer.Email))
            {
                ModelState.AddModelError("", "Customer already exists");
                return StatusCode(422, ModelState);
            }
            try
            {
                var createdCustomer = _customerService.CreateCustomer(customer);
                return Ok(createdCustomer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost("Login")]
        public ActionResult LoginCustomer([FromBody] CustomerDto loginCustomer)
        {
           /* Customer customer = _repository.GetByCondition(e => e.Email == loginCustomer.Email);

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
                _configuration.GetSection("Jwt:TokenKey").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;*/
           var response = _customerService.LoginCustomer(loginCustomer);
            return Ok(response);
        }


        [HttpGet("Profile"), Authorize]
        public ActionResult GetProfile()
        {

            var emailClaim = User.FindFirst(ClaimTypes.Email);
            var roleClaim = User.FindAll(ClaimTypes.Role);
            if (emailClaim == null)
            {
                return Unauthorized("No email claim found.");
            }

            var email = emailClaim.Value;
            var roles = roleClaim.Select(r => r.Value).ToList();

            var profileResponse = _customerService.GetProfile(email, roles);
           /* if (customer == null)
            {
                return NotFound("Customer profile not found.");
            }


            var profileResponse = new
            {
                customer.Name,
                customer.Email,
                customer.MembershipTypeId,
                customer.CustomerRoles.Count,
                Roles = roles

            };*/

            return Ok(profileResponse);
        }
        [HttpPatch("AddRole")]
        public ActionResult<Customer> AddRoleToCustomer([FromBody] AddRoleToCustomerDto model)
        {
            /*var customer = _repository.GetByCondition(e => e.Email == model.CustomerEmail);

            if (!_repository.Exists(c => c.Email == model.CustomerEmail))
            {
                ModelState.AddModelError("", "The customer does not exist");
                return StatusCode(422, ModelState);
            }

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
            return Ok(roleDtos);*/
            try
            {
                var rolesDto  = _customerService.AddRoleToCustomer(model);
                return Ok(rolesDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
           
        }

    [HttpDelete("{id}")]
    public IActionResult DeleteCustomer([FromRoute] int id)
    {
       /* if (!_repository.Exists(i => i.Id == id))
        {
            return NotFound("Customer not found.");
        }

        var customer = _repository.Get(id);
        if (customer == null)
        {
            return NotFound("Customer not found.");
        }

        _repository.Delete(customer);
        _repository.Save();
        return NoContent();*/
       _customerService.DeleteCustomer(id);
        return Ok();

    }
    [HttpPut("{id}")]
    public ActionResult UpdateCustomer(int id, UpdateCustomerDto updatedCustomer)
    {
            /*if (!_repository.Exists(i => i.Id == id))
            {
                return NotFound("Customer not found.");
            }

            var existingCustomer = _repository.Get(id);
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

            _repository.Update(existingCustomer);
            _repository.Save();

            return Ok(existingCustomer);*/
            var customer = _customerService.UpdateCustomer(id, updatedCustomer);
            return Ok(customer);
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

