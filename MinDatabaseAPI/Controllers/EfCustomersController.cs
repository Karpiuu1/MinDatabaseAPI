using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Interface;
using MinDatabaseAPI.Models;
using MinDatabaseAPI.Services;
using System.Collections.Generic;
using System.Data;


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
        [Authorize(Roles = "Admin")]
        public IActionResult AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state in AddCustomer.");
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
        [HttpDelete("{addressId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAddress(int addressId)
        {
            try
            {
                _customerService.DeleteAddress(addressId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteAddress: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
