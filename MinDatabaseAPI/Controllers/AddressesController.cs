using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;
using MinDatabaseAPI.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressesController : ControllerBase
    {
        private readonly SqlCustomerService _customerService;

        public AddressesController(SqlCustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("{customerId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAddresses(int customerId, IEnumerable<Address> addresses)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            _customerService.InsertAddresses(customerId, addresses);
            return Ok();
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
