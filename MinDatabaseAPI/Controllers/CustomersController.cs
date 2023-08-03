using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;
using MinDatabaseAPI.Models;


namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly SqlCustomerService _customerService;

        public CustomersController(SqlCustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            return Ok(customers);
        }
        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id) 
        {
            var customer = _customerService.GetAddressesByCustomerId(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
        [HttpGet("{customerId}/addresses")]
        public IActionResult GetAddressesByCustomerId(int customerId)
        {
            var addresses = _customerService.GetAddressesByCustomerId(customerId);
            return Ok(addresses);
        }
        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var newCustomerId = _customerService.InsertCustomer(customer);
            return CreatedAtAction(nameof(GetCustomerById), new {id = newCustomerId}, customer);
        }
    }
}
