using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Entities;
using MinDatabaseAPI.Services;

namespace MinDatabaseAPI.Controllers;
[ApiController]
[Route("api/customers")]

public class CustomerController : ControllerBase
{
    private readonly CustomerService customerService;

    public CustomerController(CustomerService customerService)
    {
        this.customerService = customerService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetAllCustomers()
    {
        var customers = customerService.GetAllCustomers();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public ActionResult<Customer> GetCustomerById(int id)
    {
        var customer = customerService.GetCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpGet("{customerId}/addresses")]
    public ActionResult<IEnumerable<Address>> GetAddressesByCustomerId(int customerId)
    {
        var addresses = customerService.GetAddressesByCustomerId(customerId);
        return Ok(addresses);
    }

    [HttpPost]
    public ActionResult InsertNewCustomer([FromBody] Customer customer)
    {
        customerService.InsertNewCustomer(customer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.ID }, customer);
    }

    [HttpPost("{customerId}/addresses")]
    public ActionResult InsertNewAddressesForCustomer(int customerId, [FromBody] List<Address> addresses)
    {
        customerService.InsertNewAddressesForCustomer(customerId, addresses);
        return CreatedAtAction(nameof(GetAddressesByCustomerId), new { customerId }, addresses);
    }

    [HttpDelete("{addressId}")]
    public ActionResult DeleteAddressById(int addressId)
    {
        customerService.DeleteAddressById(addressId);
        return NoContent();
    }
}