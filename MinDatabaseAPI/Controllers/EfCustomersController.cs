using Microsoft.AspNetCore.Mvc;
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

        public EfCustomersController(EfCustomerService customerService)
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
        public IActionResult AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var newCustomerId = _customerService.InsertCustomer(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomerId }, customer);
        }
    }
}
