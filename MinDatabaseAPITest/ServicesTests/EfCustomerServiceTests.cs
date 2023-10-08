using Microsoft.EntityFrameworkCore;
using MinDatabaseAPI.Models;
using MinDatabaseAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinDatabaseAPITest.ServicesTests
{
    [TestClass]
    public class EfCustomerServiceTests
    {
        private CustomerDbContext _dbContext;
        private EfCustomerService _customerService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new CustomerDbContext(options);
            _customerService = new EfCustomerService(_dbContext);

            // Dodaj przykładowe dane do bazy danych
            var customers = new List<Customer>
        {
            new Customer { FirstName = "John", LastName = "Doe", Username = "JDoe", Password = "Jdoe123", Role = "User" },
            new Customer { FirstName = "Jane", LastName = "Smith", Username = "JSmith", Password = "Jsmith123", Role = "User" }
        };
            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();
        }
        [TestMethod]
        public void GetAllCustomers_ReturnsAllCustomers()
        {
            var customers = _customerService.GetAllCustomers().ToList();

            Assert.AreEqual(2, customers.Count);
        }
        [TestMethod]
        public void GetCustomerById_ExistingId_ReturnsCustomer()
        {
            var customer = _customerService.GetCustomerById(1);

            Assert.IsNotNull(customer);
            Assert.AreEqual("John", customer.FirstName);
            Assert.AreEqual("Doe", customer.LastName);
        }
        [TestMethod]
        public void GetCustomerById_NonExistingId_ReturnsNull()
        {
            var customer = _customerService.GetCustomerById(100);

            Assert.IsNull(customer);
        }
        [TestMethod]
        public void GetAddressesByCustomerId_ReturnsCustomerAddresses()
        {
            var customer = _customerService.GetCustomerById(1);

            var addresses = _customerService.GetAddressesByCustomerId(customer.Id).ToList();

            Assert.AreEqual(0, addresses.Count);
        }
        [TestMethod]
        public void InsertCustomer_AddsNewCustomer()
        {
            var newCustomer = new Customer { FirstName = "Alice", LastName = "Johnson", Username = "AJohnson", Password = "Ajohnson123", Role = "User" };
            var customerId = _customerService.InsertCustomer(newCustomer);

            var retrievedCustomer = _dbContext.Customers.Find(customerId);

            Assert.IsNotNull(retrievedCustomer);
            Assert.AreEqual("Alice", retrievedCustomer.FirstName);
            Assert.AreEqual("Johnson", retrievedCustomer.LastName);
        }
        [TestMethod]
        public void InsertAddresses_AddsAddressesToCustomer()
        {
            var customer = _customerService.GetCustomerById(1);

            var newAddresses = new List<Address>
        {
            new Address { Street = "123 Main St", City = "Sample City", PostalCode = "12345" },
            new Address { Street = "456 Elm St", City = "Another City", PostalCode = "54321" }
        };

            _customerService.InsertAddresses(customer.Id, newAddresses);

            var addresses = _dbContext.Addresses.Where(a => a.CustomerId == customer.Id).ToList();

            Assert.AreEqual(2, addresses.Count);
        }
        [TestMethod]
        public void DeleteAddress_RemovesAddress()
        {
            var customer = _customerService.GetCustomerById(1);
            var newAddress = new Address { CustomerId = customer.Id, Street = "123 Main St", City = "Sample City", PostalCode = "12345" };
            _dbContext.Addresses.Add(newAddress);
            _dbContext.SaveChanges();

            _customerService.DeleteAddress(newAddress.AddressId);

            var address = _dbContext.Addresses.Find(newAddress.AddressId);

            Assert.IsNull(address);
        }
        [TestCleanup]
        public void Cleanup()
        {
         
            _dbContext.Database.EnsureDeleted();
        }

    }
}
