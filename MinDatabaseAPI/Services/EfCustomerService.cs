using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using MinDatabaseAPI.Models;
namespace MinDatabaseAPI.Services
{
    public class EfCustomerService
    {
        private readonly CustomerDbContext _dbContext;

        public EfCustomerService(CustomerDbContext dbContext) 
        {
            _dbContext = dbContext;   
        }

        public IEnumerable<Customer> GetAllCustomers() 
        {
            return _dbContext.Customers.ToList();        
        }

        public Customer GetCustomerById(int id)
        {
            return _dbContext.Customers.SingleOrDefault(c => c.Id == id);
        }

        public IEnumerable<Address> GetAddressesByCustomerId(int customerId)
        {
            return _dbContext.Addresses.Where(a => a.CustomerId == customerId).ToList();
        }

        public int InsertCustomer(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();
            return customer.Id;
        }


        public void InsertAddresses(int customerId, IEnumerable<Address> addresses)
        {
            foreach (var address in addresses)
            {
                address.CustomerId = customerId;
            }
            _dbContext.Addresses.AddRange(addresses);
            _dbContext.SaveChanges();
        }

        public void DeleteAddress(int addressId)
        {
            var addressToDelete = _dbContext.Addresses.Find(addressId);
            if (addressToDelete != null)
            {
                _dbContext.Addresses.Remove(addressToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}
