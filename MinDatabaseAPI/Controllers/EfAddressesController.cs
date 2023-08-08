using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;
using MinDatabaseAPI.Models;
using System.Collections.Generic;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/efaddress")]
    public class EfAddressesController : ControllerBase
    {
        private readonly EfCustomerService _customerService;

        public EfAddressesController(EfCustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("{customerId}")]
        public IActionResult AddAddresses(int customerId, IEnumerable<Address> addresses)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            _customerService.InsertAddresses(customerId, addresses);
            return Ok();
        }
        [HttpDelete("{addressId}")]
        public IActionResult DeleteAddress(int addressId)
        {
            _customerService.DeleteAddress(addressId);
            return NoContent();
        }
    }
}
