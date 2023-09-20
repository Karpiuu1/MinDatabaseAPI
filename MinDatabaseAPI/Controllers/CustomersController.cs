using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;
using MinDatabaseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using MinDatabaseAPI.Interface;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/customer")]

    public class CustomersController : ControllerBase
    {
        private readonly SqlCustomerService _customerService;
        private readonly ILoggerService _logger;

        public CustomersController(SqlCustomerService customerService, ILoggerService logger)
        {
            _customerService = customerService;
            _logger = logger;
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
            try
            {
                var customer = _customerService.GetCustomerById(id);

                if (customer == null)
                {
                    _logger.LogError($"Customer not found with ID: {id}");
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCustomerById: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
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
            try
            {

                var newCustomerId = _customerService.InsertCustomer(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomerId }, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddCustomer: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                var customer = _customerService.GetCustomerById(id);

                if (customer == null)
                {
                    _logger.LogError($"Customer not found with ID: {id}");
                    return NotFound();
                }

                _customerService.DeleteCustomer(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteCustomer: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
