using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;
using MinDatabaseAPI.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using MinDatabaseAPI.Interface;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressesController : ControllerBase
    {
        private readonly SqlCustomerService _customerService;
        private readonly ILoggerService _logger;

        public AddressesController(SqlCustomerService customerService, ILoggerService logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpPost("{customerId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAddresses(int customerId, IEnumerable<Address> addresses)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state in AddAddresses.");
                return BadRequest();
            }

            try
            {
                _customerService.InsertAddresses(customerId, addresses);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddAddresses: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{customerId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAddress(int addressId)
        {
            _customerService.DeleteAddress(addressId);
            return NoContent();
        }
    }
}
