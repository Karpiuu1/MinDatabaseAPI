using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Interface;
using MinDatabaseAPI.Models;
using MinDatabaseAPI.Services;
using System.Collections.Generic;


namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/efcustomer")]
    public class EfCustomersController : ControllerBase
    {
        private readonly EfCustomerService _customerService;
        private readonly ILoggerService _logger;

        public EfCustomersController(EfCustomerService customerService, ILoggerService logger)
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
        public IActionResult AddCustomer(Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state in AddCustomer.");
                    return BadRequest();
                }

                var newCustomerId = _customerService.InsertCustomer(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomerId }, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddCustomer: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
