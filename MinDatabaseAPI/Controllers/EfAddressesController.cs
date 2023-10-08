using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;
using MinDatabaseAPI.Models;
using System.Collections.Generic;
using MinDatabaseAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/efaddress")]
    public class EfAddressesController : ControllerBase
    {
        private readonly EfCustomerService _customerService;
        private readonly ILoggerService _logger;
        public EfAddressesController(EfCustomerService customerService, ILoggerService logger)
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
