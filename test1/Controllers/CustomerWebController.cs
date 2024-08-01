using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        //public static Customer customerObj = new Customer();

        public CustomerWebController(ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
        }

        [HttpGet("{GetString}")]
        //[Route("GetString")]
        public string GetString()
        {
            return "working";
        }
        [HttpGet("GetCustomers"), Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(customers);
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
        [HttpPost("CreateCustomer")]
        public ActionResult<Customer> CreateCustomer([FromBody] CustomerDto customer)
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
           /* customerObj.Password = hashedPassword;
            customerObj.Email = customer.Email;
            customerObj.Name = customer.Name;
            customerObj.MembershipTypeId = customer.MembershipTypeId;*/
            var createdCustomer = _customerRepository.CreateCustomer(customer);

            return Ok(createdCustomer);
        }
        [HttpPost("LoginCustomer")]
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
            return Ok(token);
        } 

        private string CreateToken(Customer customer)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, customer.Name)
                };

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

    }
}
