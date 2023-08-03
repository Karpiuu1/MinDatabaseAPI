using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;
using MinDatabaseAPI.Models;


namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : ControllerBase
    {
        private readonly SqlCustomerService _customerService;

        public AddressesController(SqlCustomerService customerService)
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

        [HttpDelete("{customerId")]
        public IActionResult DeleteAddress(int addressId)
        {
            _customerService.DeleteAddress(addressId);
            return NoContent();
        }
    }
}
