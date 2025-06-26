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

    public class CustomerWebController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        public IMapper _mapper;
        private readonly ICustomerService _customerService;

        public CustomerWebController(IConfiguration configuration, IMapper mapper, ICustomerService customerService)
        {
           // _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
            _customerService = customerService;
            /*_roleRepository = roleRepo;
            _customerRoleRepo = crRole;*/
        }

        [HttpGet("{GetString}")]
        public string GetString()
        {
            return "working";
        }
        //[AuthorizeRole(Roles ="Admin")]
        [HttpGet("GetCustomers")]
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
        [HttpGet("GetCustomerById/{id}")]
        public ActionResult GetCustomerById(int id)
        {
          
           var customer = _customerService.GetCustomerById(id);
           return Ok(customer);
        }

        [HttpPost("Register")]
        public ActionResult<Customer> CreateCustomer([FromBody] CustomerDtoSignUp customer)
        {
       
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

            return Ok(profileResponse);
        }
        [HttpPatch("AddRole")]
        public ActionResult<Customer> AddRoleToCustomer([FromBody] AddRoleToCustomerDto model)
        {
            
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

    [HttpDelete("DeleteCustomer/{id}")]
    public IActionResult DeleteCustomer([FromRoute] int id)
    {
       _customerService.DeleteCustomer(id);
        return Ok();

    }
    [HttpPut("UpdateCustomer/{id}")]
    public ActionResult UpdateCustomer(int id, UpdateCustomerDto updatedCustomer)
    {
            var customer = _customerService.UpdateCustomer(id, updatedCustomer);
            return Ok(customer);
    }

        }
    }

