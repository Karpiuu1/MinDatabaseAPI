using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;
using MinDatabaseAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/customer")]
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
            var customer = _customerService.GetCustomerById(id);
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
        [Authorize(Roles = "Admin")]
        public IActionResult AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var newCustomerId = _customerService.InsertCustomer(customer);
            return CreatedAtAction(nameof(GetCustomerById), new {id = newCustomerId}, customer);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            _customerService.DeleteCustomer(id);

            return NoContent();

        }
    }
}
